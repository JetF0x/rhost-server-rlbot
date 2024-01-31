﻿using System.Drawing;

namespace RLBotSecret.Conversion
{
    internal class ColorSwatches
    {
        internal static Color GetPrimary(uint colorId, uint team)
        {
            var swatches = team == 0 ? blueSwatches : orangeSwatches;
            return Color.FromArgb(255, swatches[colorId, 0], swatches[colorId, 1], swatches[colorId, 2]);
        }

        internal static Color GetSecondary(uint colorId)
        {
            var swatches = secondarySwatches;
            return Color.FromArgb(255, swatches[colorId, 0], swatches[colorId, 1], swatches[colorId, 2]);
        }

        // These were generated by Darxeal by pulling RGB values from an in-game screenshot
        // of the color pickers. He made a python script to do it.
        private static int[,] blueSwatches = new int[70, 3]
        {
            { 80, 127, 57 },
            { 57, 127, 63 },
            { 57, 127, 100 },
            { 57, 125, 127 },
            { 57, 107, 127 },
            { 57, 93, 127 },
            { 57, 79, 127 },
            { 57, 66, 127 },
            { 76, 57, 127 },
            { 81, 57, 127 },
            { 101, 178, 62 },
            { 62, 178, 72 },
            { 62, 178, 134 },
            { 62, 174, 178 },
            { 62, 145, 178 },
            { 62, 122, 178 },
            { 62, 99, 178 },
            { 62, 77, 178 },
            { 93, 62, 178 },
            { 103, 62, 178 },
            { 114, 229, 57 },
            { 57, 229, 71 },
            { 57, 229, 163 },
            { 57, 223, 229 },
            { 57, 180, 229 },
            { 57, 146, 229 },
            { 57, 111, 229 },
            { 57, 80, 229 },
            { 103, 57, 229 },
            { 117, 57, 229 },
            { 92, 252, 12 },
            { 12, 252, 32 },
            { 12, 252, 160 },
            { 12, 244, 252 },
            { 12, 184, 252 },
            { 12, 136, 252 },
            { 12, 88, 252 },
            { 12, 44, 252 },
            { 76, 12, 252 },
            { 96, 12, 252 },
            { 74, 204, 10 },
            { 10, 204, 26 },
            { 10, 204, 129 },
            { 10, 197, 204 },
            { 10, 149, 204 },
            { 10, 110, 204 },
            { 10, 71, 204 },
            { 10, 36, 204 },
            { 61, 10, 204 },
            { 78, 10, 204 },
            { 60, 165, 8 },
            { 8, 165, 21 },
            { 8, 165, 105 },
            { 8, 160, 165 },
            { 8, 121, 165 },
            { 8, 89, 165 },
            { 8, 58, 165 },
            { 8, 29, 165 },
            { 50, 8, 165 },
            { 63, 8, 165 },
            { 46, 127, 6 },
            { 6, 127, 16 },
            { 6, 127, 81 },
            { 6, 123, 127 },
            { 6, 93, 127 },
            { 6, 68, 127 },
            { 6, 44, 127 },
            { 6, 22, 127 },
            { 38, 6, 127 },
            { 48, 6, 127 }
        };

        private static int[,] orangeSwatches = new int[70, 3]
        {
            { 127, 127, 57 },
            { 127, 112, 57 },
            { 127, 99, 57 },
            { 127, 90, 57 },
            { 127, 84, 57 },
            { 127, 78, 57 },
            { 127, 71, 57 },
            { 127, 57, 57 },
            { 127, 57, 81 },
            { 127, 57, 92 },
            { 178, 178, 62 },
            { 178, 153, 62 },
            { 178, 132, 62 },
            { 178, 116, 62 },
            { 178, 106, 62 },
            { 178, 97, 62 },
            { 178, 85, 62 },
            { 178, 62, 62 },
            { 178, 62, 103 },
            { 178, 62, 120 },
            { 229, 229, 57 },
            { 229, 192, 57 },
            { 229, 160, 57 },
            { 229, 137, 57 },
            { 229, 123, 57 },
            { 229, 109, 57 },
            { 229, 91, 57 },
            { 229, 57, 57 },
            { 229, 57, 117 },
            { 229, 57, 143 },
            { 252, 252, 12 },
            { 252, 200, 12 },
            { 252, 156, 12 },
            { 252, 124, 12 },
            { 252, 104, 12 },
            { 252, 84, 12 },
            { 252, 60, 12 },
            { 252, 12, 12 },
            { 252, 12, 96 },
            { 252, 12, 132 },
            { 204, 204, 10 },
            { 204, 162, 10 },
            { 204, 126, 10 },
            { 204, 100, 10 },
            { 204, 84, 10 },
            { 204, 68, 10 },
            { 204, 48, 10 },
            { 204, 10, 10 },
            { 204, 10, 78 },
            { 204, 10, 107 },
            { 165, 165, 8 },
            { 165, 131, 8 },
            { 165, 102, 8 },
            { 165, 81, 8 },
            { 165, 68, 8 },
            { 165, 55, 8 },
            { 165, 39, 8 },
            { 165, 8, 8 },
            { 165, 8, 63 },
            { 165, 8, 87 },
            { 127, 127, 6 },
            { 127, 101, 6 },
            { 127, 79, 6 },
            { 127, 62, 6 },
            { 127, 52, 6 },
            { 127, 42, 6 },
            { 127, 30, 6 },
            { 127, 6, 6 },
            { 127, 6, 48 },
            { 127, 6, 66 }
        };

        private static int[,] secondarySwatches = new int[105, 3]
        {
            { 229, 229, 229 },
            { 255, 127, 127 },
            { 255, 159, 127 },
            { 255, 207, 127 },
            { 239, 255, 127 },
            { 175, 255, 127 },
            { 127, 255, 127 },
            { 127, 255, 178 },
            { 127, 233, 255 },
            { 127, 176, 255 },
            { 127, 136, 255 },
            { 174, 127, 255 },
            { 229, 127, 255 },
            { 255, 127, 208 },
            { 255, 127, 148 },
            { 191, 191, 191 },
            { 255, 89, 89 },
            { 255, 130, 89 },
            { 255, 192, 89 },
            { 234, 255, 89 },
            { 151, 255, 89 },
            { 89, 255, 89 },
            { 89, 255, 155 },
            { 89, 227, 255 },
            { 89, 152, 255 },
            { 89, 100, 255 },
            { 150, 89, 255 },
            { 221, 89, 255 },
            { 255, 89, 194 },
            { 255, 89, 116 },
            { 153, 153, 153 },
            { 255, 50, 50 },
            { 255, 101, 50 },
            { 255, 178, 50 },
            { 229, 255, 50 },
            { 127, 255, 50 },
            { 50, 255, 50 },
            { 50, 255, 132 },
            { 50, 220, 255 },
            { 50, 129, 255 },
            { 50, 64, 255 },
            { 125, 50, 255 },
            { 214, 50, 255 },
            { 255, 50, 180 },
            { 255, 50, 85 },
            { 102, 102, 102 },
            { 255, 0, 0 },
            { 255, 63, 0 },
            { 255, 159, 0 },
            { 223, 255, 0 },
            { 95, 255, 0 },
            { 0, 255, 0 },
            { 0, 255, 102 },
            { 0, 212, 255 },
            { 0, 97, 255 },
            { 0, 17, 255 },
            { 93, 0, 255 },
            { 204, 0, 255 },
            { 255, 0, 161 },
            { 255, 0, 42 },
            { 63, 63, 63 },
            { 178, 0, 0 },
            { 178, 44, 0 },
            { 178, 111, 0 },
            { 156, 178, 0 },
            { 66, 178, 0 },
            { 0, 178, 0 },
            { 0, 178, 71 },
            { 0, 148, 178 },
            { 0, 68, 178 },
            { 0, 11, 178 },
            { 65, 0, 178 },
            { 142, 0, 178 },
            { 178, 0, 113 },
            { 178, 0, 29 },
            { 38, 38, 38 },
            { 102, 0, 0 },
            { 102, 25, 0 },
            { 102, 63, 0 },
            { 89, 102, 0 },
            { 38, 102, 0 },
            { 0, 102, 0 },
            { 0, 102, 40 },
            { 0, 84, 102 },
            { 0, 39, 102 },
            { 0, 6, 102 },
            { 37, 0, 102 },
            { 81, 0, 102 },
            { 102, 0, 64 },
            { 102, 0, 17 },
            { 5, 5, 5 },
            { 51, 0, 0 },
            { 51, 12, 0 },
            { 51, 31, 0 },
            { 44, 51, 0 },
            { 19, 51, 0 },
            { 0, 51, 0 },
            { 0, 51, 20 },
            { 0, 42, 51 },
            { 0, 19, 51 },
            { 0, 3, 51 },
            { 18, 0, 51 },
            { 40, 0, 51 },
            { 51, 0, 32 },
            { 51, 0, 8 }
        };
    }
}
