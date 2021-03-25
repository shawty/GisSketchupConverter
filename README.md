# GisSketchupConverter
A simple (Windows X64 only) drag and drop converter, that will take a shape file encoded using OSGB36 and convert it to a Sketchup 2017 compatible 2D surface file.

This program was written to go with the "History of Consett Steelworks" web mapping session for March 2021, it will convert a single object shape file into a single 2D flat plane ready for import into Sketchup 2017.

The SKP file produced will be fully geo-referenced to the exact same location on the planet as the source shape file, the source shape file MUST be created using OSGB36 (EPSG:27700) UK National Grid co-ordinate space, and the resulting model will be suitable for exporting from Sketchup as a KMZ model ready to be loaded directly into Google Earth.
