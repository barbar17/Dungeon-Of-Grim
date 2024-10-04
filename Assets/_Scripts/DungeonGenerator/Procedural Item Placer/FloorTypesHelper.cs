using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class FloorTypesHelper
{
    public static HashSet<int> NearWallTilesUp = new HashSet<int>
    {

        0b01111111,
        0b00111111,
        0b01111110,
        0b00111110,
    };

    public static HashSet<int> NearWallTilesDown = new HashSet<int>
    {
        0b11110111,
        0b11110011,
        0b11100111,
        0b11100011,
    };

    public static HashSet<int> NearWallTilesLeft = new HashSet<int>
    {
        0b11111101,
        0b11111001,
        0b11111100,
        0b11111000,
    };

    public static HashSet<int> NearWallTilesRight = new HashSet<int>
    {
        0b11011111,
        0b10011111,
        0b11001111,
        0b10001111,

    };

    public static HashSet<int> NarrowTiles = new HashSet<int>
    {
        0b01101111,
        0b10110111,
        0b11011011,
        0b11101101,
        0b11110110,
        0b01111011,
        0b10111101,
        0b11011110,
        0b01110111,
        0b10111011,
        0b11011101,
        0b11101110,
        0b01110111,
        0b10111011,
        0b11011101,
        0b11101110,
        0b10111110,
        0b10101111,
        0b11101011,
        0b11111010,
        0b00101111,
        0b11001011,
        0b11110010,
        0b10111100,
        0b01111010,
        0b10011110,
        0b10100111,
        0b11101001,
        0b01110110,
        0b10011101,
        0b01100111,
        0b11011001,
        0b00111011,
        0b11001110,
        0b10110011,
        0b01110110,
        0b01101110,
        0b10011011,
        0b11100110,
        0b10111001,
        0b10011101,
        0b01100111,
        0b11011001,
        0b01110110,
        0b11011100,
        0b00110111,
        0b11001101,
        0b01110011,
        0b10011110,
        0b10100111,
        0b11101001,
        0b01111010,
        0b10111100,
        0b00101111,
        0b11001011,
        0b11110010,
        0b11001110,
        0b10110011,
        0b11101100,
        0b00111011,
        0b10111001,
        0b01101110,
        0b10011011,
        0b11100110,
        0b10101010,
        0b11111110,
        0b10111111,
    };

    public static HashSet<int> CornerTiles = new HashSet<int>
    {
        0b01011111,
        0b11010111,
        0b11110101,
        0b01111101,
        0b00011111,
        0b11000111,
        0b11110001,
        0b01111100,
        0b00111101,
        0b01001111,
        0b11010011,
        0b11110100,
        0b10010111,
        0b11100101,
        0b01111001,
        0b01011110,
        0b00001111,
        0b11000011,
        0b11110000,
        0b00111100,
        0b01111000,
        0b00011110,
        0b10000111,
        0b11100001,
        0b10000011,
        0b11100000,
        0b00111000,
        0b00001110,
    };

    public static HashSet<int> InnerTiles = new HashSet<int>
    {
        0b11111111,
        0b11101111,
        0b11111011,
        0b11111111,

    };
}