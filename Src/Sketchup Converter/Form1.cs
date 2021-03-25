using GeoAPI.Geometries;
using NetTopologySuite.IO.ShapeFile.Extended;
using ProjNet.CoordinateSystems;
using ProjNet.CoordinateSystems.Transformations;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace Sketchup_Converter
{
  public partial class Form1 : Form
  {
    public Form1()
    {
      InitializeComponent();
      AllowDrop = true;
    }

    private void Form1_DragEnter(object sender, DragEventArgs e)
    {
      if (e.Data.GetDataPresent(DataFormats.FileDrop))
      {
        e.Effect = DragDropEffects.Copy;
      }
      else
      {
        e.Effect = DragDropEffects.None;
      }
    }

    private void Form1_DragDrop(object sender, DragEventArgs e)
    {
      if (e.Data.GetDataPresent(DataFormats.FileDrop))
      {
        var filePaths = (string[])(e.Data.GetData(DataFormats.FileDrop));
        string shapeFile = filePaths.FirstOrDefault();

        if(shapeFile == null)
        {
          MessageBox.Show("Sorry cannot load that file.");
        }

        if(!shapeFile.ToLower().Contains(".shp"))
        {
          MessageBox.Show("Sorry, but that doesn't appear to be a shape file, it does not end with a 'shp' extension.");
          return;
        }

        string fullPath = Path.GetDirectoryName(shapeFile);
        string fileName = Path.GetFileNameWithoutExtension(shapeFile);

        ConvertToSketchup(fullPath, fileName);

      }
    }

    private void ConvertToSketchup(string path, string name)
    {

      // Read source shape file
      ShapeDataReader reader = new ShapeDataReader($"{Path.Combine(path, name)}.shp");

      var mbr = reader.ShapefileBounds;

      var result = reader.ReadByMBRFilter(mbr);

      double bx;
      double by;

      var shapeFeature = result.ToList()[0];

      bx = shapeFeature.BoundingBox.MinX;
      by = shapeFeature.BoundingBox.MinY;

      List<Coordinate> coordsForSketchup = new List<Coordinate>();

      foreach (var co in shapeFeature.Geometry.Coordinates)
      {
        coordsForSketchup.Add(new Coordinate(co.X - bx, co.Y - by, 0));
      }

      var csFactory = new CoordinateSystemFactory();
      var ctFactory = new CoordinateTransformationFactory();

      var osgb36 = csFactory.CreateFromWkt("PROJCS[\"OSGB 1936 / British National Grid\",GEOGCS[\"OSGB 1936\",DATUM[\"OSGB_1936\",SPHEROID[\"Airy 1830\",6377563.396,299.3249646,AUTHORITY[\"EPSG\",\"7001\"]],TOWGS84[446.448,-125.157,542.06,0.15,0.247,0.842,-20.489],AUTHORITY[\"EPSG\",\"6277\"]],PRIMEM[\"Greenwich\",0,AUTHORITY[\"EPSG\",\"8901\"]],UNIT[\"degree\",0.0174532925199433,AUTHORITY[\"EPSG\",\"9122\"]],AUTHORITY[\"EPSG\",\"4277\"]],PROJECTION[\"Transverse_Mercator\"],PARAMETER[\"latitude_of_origin\",49],PARAMETER[\"central_meridian\",-2],PARAMETER[\"scale_factor\",0.9996012717],PARAMETER[\"false_easting\",400000],PARAMETER[\"false_northing\",-100000],UNIT[\"metre\",1,AUTHORITY[\"EPSG\",\"9001\"]],AXIS[\"Easting\",EAST],AXIS[\"Northing\",NORTH],AUTHORITY[\"EPSG\",\"27700\"]]");
      var wgs84 = csFactory.CreateFromWkt("GEOGCS[\"WGS 84\",DATUM[\"WGS_1984\",SPHEROID[\"WGS 84\",6378137,298.257223563,AUTHORITY[\"EPSG\",\"7030\"]],AUTHORITY[\"EPSG\",\"6326\"]],PRIMEM[\"Greenwich\",0,AUTHORITY[\"EPSG\",\"8901\"]],UNIT[\"degree\",0.0174532925199433,AUTHORITY[\"EPSG\",\"9122\"]],AUTHORITY[\"EPSG\",\"4326\"]]");

      var transform = ctFactory.CreateFromCoordinateSystems(osgb36, wgs84);

      var lowerLeftOSGB = new Coordinate(bx, by);

      var lowerLeftWGS = transform.MathTransform.Transform(lowerLeftOSGB);

      // Create a sketchup file with the building outline in (Not yet georefed)

      SketchUpNET.SketchUp skp = new SketchUpNET.SketchUp();

      skp.Latitude = lowerLeftWGS.Y;
      skp.Longitude = lowerLeftWGS.X;

      //skp.LoadModel("NewSquare2.skp");

      // Nothing is initialised by default, and we need to init it all even stuff we don't use
      skp.Components = new System.Collections.Generic.Dictionary<string, SketchUpNET.Component>();
      skp.Curves = new System.Collections.Generic.List<SketchUpNET.Curve>();
      skp.Groups = new System.Collections.Generic.List<SketchUpNET.Group>();
      skp.Instances = new System.Collections.Generic.List<SketchUpNET.Instance>();
      skp.Materials = new System.Collections.Generic.Dictionary<string, SketchUpNET.Material>();

      skp.Layers = new System.Collections.Generic.List<SketchUpNET.Layer>();
      skp.Layers.Add(new SketchUpNET.Layer("Layer0"));

      skp.Edges = new System.Collections.Generic.List<SketchUpNET.Edge>();

      for (int current = 0; current < (coordsForSketchup.Count - 1); current++)
      {
        skp.Edges.Add(new SketchUpNET.Edge(
          new SketchUpNET.Vertex(coordsForSketchup[current].X, coordsForSketchup[current].Y, 0),
          new SketchUpNET.Vertex(coordsForSketchup[current + 1].X, coordsForSketchup[current + 1].Y, 0),
          skp.Layers[0].Name
        ));
      }

      var surface = new SketchUpNET.Surface();

      surface.Layer = skp.Layers[0].Name;
      surface.Normal = new SketchUpNET.Vector(0, 0, -1);

      surface.OuterEdges = new SketchUpNET.Loop();
      surface.OuterEdges.Edges = new System.Collections.Generic.List<SketchUpNET.Edge>();

      foreach (var edge in skp.Edges)
      {
        surface.OuterEdges.Edges.Add(edge);
      }

      surface.Vertices = new System.Collections.Generic.List<SketchUpNET.Vertex>();

      for (int current = 0; current < (coordsForSketchup.Count - 1); current++)
      {
        surface.Vertices.Add(new SketchUpNET.Vertex(coordsForSketchup[current].X, coordsForSketchup[current].Y, 0));
      }

      skp.Surfaces = new System.Collections.Generic.List<SketchUpNET.Surface>();
      skp.Surfaces.Add(surface);

      skp.WriteNewModel("Temp.skp");
      skp.SaveAs("Temp.skp", SketchUpNET.SKPVersion.V2017, $"{Path.Combine(path, name)}.skp");

      File.Delete("Temp.skp");

    }

  }
}
