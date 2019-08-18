# KEGG.molDraw

[![DOI](https://zenodo.org/badge/DOI/10.5281/zenodo.2532222.svg)](https://doi.org/10.5281/zenodo.2532222)

Drawing chemical compound structures and glycan structures with capabilities to search against the KEGG databases

This application source project required reference to sciBASIC.NET framework: https://github.com/xieguigang/sciBASIC

![](AppScreen.png)

### Example

```vbnet
' Draw a NADPH molecule 2D structure in Extension pipline style.
Call IO _
    .LoadKCF("./DATA/NADPH.txt") _
    .Draw() _
    .Save("./DATA/NADPH.png")
```

![](./DATA/NADPH.png)
