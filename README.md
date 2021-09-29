# World Generator
 2D Procedural World Generator. Built in Unity.

This project gave me a chance to experiment with procedural generation by creating a 2D map generator from scratch, and stitching together the image using Unity. Notable features include: layouts as generator inputs, biome and humidity mapping, mountain placement, and a simplified heightmap.

**[Click for Gallery](https://stephentherianos.com/worldgen)**

# Generation Pipeline

**Layout Step**

A map sketch is either supplied or generated based on some input parameters, such as map size, number of continents, continent size, and number of mountain ranges. A “sketch” is any 2D array where each cell is mapped to a specific region of the full-size world coordinates. The value of each cell dictates how many times to use a particular generation tool on that region of the coordinate map. For example, a 2x2 layout array would map to each quadrant of the world coordinates.

My world sizes in these examples are about 150x100, and sketches typically between 5x5 and 11x11.

**Generation Step**

Generation is additive starting from a completely ocean world. Splotches of land are painted on according to the value of each cell in the “sketch” array, at a random location within the world-coordinates they are mapped to. A subtractive pass follows, in order to reinforce coastlines and encourage interesting terrain. Afterwards, a smoothing pass reduces the number of jagged artifacts to an acceptable level.

**Environmental Step**

Several “heatmap” arrays are generated as source material for the various environmental passes. A coastal heatmap records how far each tile is from the nearest coast. A mountain map places randomized mountain ranges and foothills with a generate-and-test approach. A rain shadow map records which tiles have reduced humidity for being in the shadow of a mountain.

These maps are combined to assign a single biome to each tile based on latitude, longitude, relative humidity, and elevation.
