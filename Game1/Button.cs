using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BitMiner
{
    class Button
    {
        Rectangle buttonRec;
        public string Text { get; set; }

        Color baseColor, hoverColor, fillColor, textColor;

        public CellType TypeSelector { get; set; }
        public int Value { get; set; }

        public Button(int x, int y, int width, int height, string text)
        {
            buttonRec = new Rectangle(x, y, width, height);
            this.Text = text;

            baseColor = Color.Black;
            hoverColor = Color.Blue;
            fillColor = baseColor;
            textColor = hoverColor;
            TypeSelector = CellType.Fill;
        }

        public Button(int x, int y, int width, int height, string text, CellType type)
        {
            buttonRec = new Rectangle(x, y, width, height);
            this.Text = text;

            baseColor = Color.Black;
            hoverColor = Color.Blue;
            fillColor = baseColor;
            textColor = hoverColor;
            TypeSelector = type;
        }

        public void Update(MouseState mouse)
        {
            fillColor = baseColor;
            textColor = hoverColor;

            if (buttonRec.Contains(mouse.Position))
            {
                textColor = baseColor;
                fillColor = hoverColor;
            }
        }

        public bool Activated(MouseState mouse)
        {
            return buttonRec.Contains(mouse.Position) && 
                   mouse.LeftButton == ButtonState.Pressed;
        }

        public CellType GetCellType(MouseState mouse)
        {
            if (Activated(mouse))
            {
                return TypeSelector;
            }
            return CellType.Fill;
        }

        public void Draw(SpriteBatch spriteBatch, Texture2D texture, SpriteFont font, bool drawValue = false)
        {
            const int EDGE = 2;
            Rectangle fill = buttonRec;
            fill.X += EDGE;
            fill.Y += EDGE;
            fill.Width -= 2 * EDGE;
            fill.Height -= 2 * EDGE;
            spriteBatch.Draw(texture, buttonRec, Color.LightBlue);
            spriteBatch.Draw(texture, fill, fillColor);
            String rendText = Text;
            if (drawValue)
                rendText += ": "+Value;
            spriteBatch.DrawString(font, rendText, new Vector2(fill.X + EDGE*3, fill.Y + EDGE* 3), textColor);
        }
    }
}
