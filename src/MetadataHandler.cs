using System;
using System.Collections.Generic;
using System.Linq;
//using System.Data;
using System.Text;
using Landis.Library.Metadata;
using Landis.Core;
using Edu.Wisc.Forest.Flel.Util;
using System.IO;
using Flel = Edu.Wisc.Forest.Flel;

namespace Landis.Extension.BiomassFuels

{
    public static class MetadataHandler
    {

        public static ExtensionMetadata Extension { get; set; }

        public static void InitializeMetadata(
            int Timestep, 
            string mapNameTemplate, 
            string pctConiferMapNameTemplate, 
            string pctDeadFirMapNameTemplate
            )
        {

            ScenarioReplicationMetadata scenRep = new ScenarioReplicationMetadata()
            {
                RasterOutCellArea = PlugIn.ModelCore.CellArea,
                TimeMin = PlugIn.ModelCore.StartTime,
                TimeMax = PlugIn.ModelCore.EndTime,
            };

            Extension = new ExtensionMetadata(PlugIn.ModelCore)
            //Extension = new ExtensionMetadata()
            {
                Name = PlugIn.ExtensionName,
                TimeInterval = Timestep,
                ScenarioReplicationMetadata = scenRep
            };

            //---------------------------------------            
            //          map outputs:         
            //---------------------------------------

            //OutputMetadata mapOut_BiomassRemoved = new OutputMetadata()
            //{
            //    Type = OutputType.Map,
            //    Name = "biomass removed",
            //    FilePath = @HarvestMapName,
            //    Map_DataType = MapDataType.Continuous,
            //    Map_Unit = FieldUnits.Mg_ha,
            //    Visualize = true,
            //};
            //Extension.OutputMetadatas.Add(mapOut_BiomassRemoved);
            
            OutputMetadata mapOut_Fuel = new OutputMetadata()
            {
                Type = OutputType.Map,
                Name = "Fuel Map",
                FilePath = MapNames.ReplaceTemplateVars(mapNameTemplate, PlugIn.ModelCore.CurrentTime),
                Map_DataType = MapDataType.Continuous,
                Visualize = true,
            };

            OutputMetadata mapOut_Confir = new OutputMetadata()
            {
                Type = OutputType.Map,
                Name = "Conifer Map",
                FilePath = MapNames.ReplaceTemplateVars(pctConiferMapNameTemplate, PlugIn.ModelCore.CurrentTime),
                Map_DataType = MapDataType.Continuous,
                Visualize = true,
            };

            OutputMetadata mapOut_Dead = new OutputMetadata()
            {
                Type = OutputType.Map,
                Name = "Dead Fir Map",
                FilePath = MapNames.ReplaceTemplateVars(pctDeadFirMapNameTemplate, PlugIn.ModelCore.CurrentTime),
                Map_DataType = MapDataType.Continuous,
                Visualize = true,
            };

            Extension.OutputMetadatas.Add(mapOut_Fuel);
            Extension.OutputMetadatas.Add(mapOut_Confir);
            Extension.OutputMetadatas.Add(mapOut_Dead);
            

            //---------------------------------------
            MetadataProvider mp = new MetadataProvider(Extension);
            mp.WriteMetadataToXMLFile("Metadata", Extension.Name, Extension.Name);
        }
        public static void CreateDirectory(string path)
        {
            //Require.ArgumentNotNull(path);
            path = path.Trim(null);
            if (path.Length == 0)
                throw new ArgumentException("path is empty or just whitespace");

            string dir = Path.GetDirectoryName(path);
            if (!string.IsNullOrEmpty(dir))
            {
                Flel.Util.Directory.EnsureExists(dir);
            }

            //return new StreamWriter(path);
            return;
        }
    }
}