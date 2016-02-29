using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CsvHelper;
using System.IO;

namespace Edge.Hyperion.Engine {
    class MapRow {
        public List<MapCell> Columns = new List<MapCell>();
    }

    class TileMap {
        public List<String[]> Rows = new List<string[]>();
        public int MapWidth = 5;
        public int MapHeight = 5;

        public TileMap(String Path) {

            String[] row;
            using (var parser = new CsvParser(File.OpenText(@"c:\test.csv")))
                while ((row = parser.Read()) != null) {
                    Rows.Add(row);
                }

            #region Sample Map Data
            //for (int y = 0; y < MapHeight; y++) {
            //    for (int x = 0; x < MapWidth; x++) {
            //        Row.Columns.Add(new MapCell(0));
            //    }
            //    Rows.Add(Row);
            //}
            //Rows[0].Columns[3].TileID = 3;
            //Rows[0].Columns[4].TileID = 3;
            //Rows[0].Columns[5].TileID = 1;
            //Rows[0].Columns[6].TileID = 1;
            //Rows[0].Columns[7].TileID = 1;

            //Rows[1].Columns[3].TileID = 3;
            //Rows[1].Columns[4].TileID = 1;
            //Rows[1].Columns[5].TileID = 1;
            //Rows[1].Columns[6].TileID = 1;
            //Rows[1].Columns[7].TileID = 1;

            //Rows[2].Columns[2].TileID = 3;
            //Rows[2].Columns[3].TileID = 1;
            //Rows[2].Columns[4].TileID = 1;
            //Rows[2].Columns[5].TileID = 1;
            //Rows[2].Columns[6].TileID = 1;
            //Rows[2].Columns[7].TileID = 1;

            //Rows[3].Columns[2].TileID = 3;
            //Rows[3].Columns[3].TileID = 1;
            //Rows[3].Columns[4].TileID = 1;
            //Rows[3].Columns[5].TileID = 2;
            //Rows[3].Columns[6].TileID = 2;
            //Rows[3].Columns[7].TileID = 2;

            //Rows[4].Columns[2].TileID = 3;
            //Rows[4].Columns[3].TileID = 1;
            //Rows[4].Columns[4].TileID = 1;
            //Rows[4].Columns[5].TileID = 2;
            //Rows[4].Columns[6].TileID = 2;
            //Rows[4].Columns[7].TileID = 2;

            //Rows[5].Columns[2].TileID = 3;
            //Rows[5].Columns[3].TileID = 1;
            //Rows[5].Columns[4].TileID = 1;
            //Rows[5].Columns[5].TileID = 2;
            //Rows[5].Columns[6].TileID = 2;
            //Rows[5].Columns[7].TileID = 2;
            #endregion Sample Map Data
        }
    }
}
