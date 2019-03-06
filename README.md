# SimpleRasterizer

## Why?
Upon implementing a paint-like sprite editor under MonoGame I found myself in a situation where usage of the built in rasterizer was no longer an option. My concern was to implement brushes and to do that efficiently I've needed to get pixels painted by the raterizer. Should you ever be in a similiar situation, you might use this rasterizer to save yourself about an hour / two. 

## What can this do?
Render pixels, lines, triangles, rectangles, quads, rounded rectangles, circles, ellipses, arcs.

[![image.png](https://i.postimg.cc/XYkYH0D5/image.png)](https://postimg.cc/ts7bJ8bR)

## How does it work?
Steeping stone of this rasterizer is function `putPixel`, currently set to accept `PaintEventArgs`, utilizing `Graphics.FillRectangle` to render the pixel. It should be very easy to replace content of this function to render pixel your desired way. Other functions use `putPixel` to render their output to the screen. Lines are implemented via Bresenham's algorithm, ellipses via Midpoint algorithm (arithmetic only), arcs via optimized Bresenham without using trigonometric functions inside loops.

## Quality / efficiency
Rasterizer doesn't use any fancy tricks to discard partially needed pixels / antialias results. Therefore speed is rather good while quality is lower than in more advanced rasterizers. 

## Example result
[![image.png](https://i.postimg.cc/NFjncRgY/image.png)](https://postimg.cc/sQbmPQQ0)

## License
MIT. Have a nice day.
