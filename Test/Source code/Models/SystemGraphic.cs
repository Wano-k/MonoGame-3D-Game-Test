﻿using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Test;

namespace RPG_Paper_Maker
{
    [Serializable]
    public class SystemGraphic
    {
        public string GraphicName;
        public bool IsRTP;
        public GraphicKind GraphicKind;
        public object[] Options;

        public enum OptionsEnum
        {
            TilesetX,
            TilesetY,
            TilesetWidth,
            TilesetHeight,
            Frames,
            Diagonal,
            Index,
        }

        // -------------------------------------------------------------------
        // Constructor
        // -------------------------------------------------------------------

        public SystemGraphic(string graphicName, bool isRTP, GraphicKind graphicKind, object[] options = null)
        {
            GraphicName = graphicName;
            IsRTP = isRTP;
            GraphicKind = graphicKind;
            Options = options;
        }

        // -------------------------------------------------------------------
        // Equals
        // -------------------------------------------------------------------

        public override bool Equals(object obj)
        {
            return IsRTP == ((SystemGraphic)obj).IsRTP && GraphicKind == ((SystemGraphic)obj).GraphicKind && GraphicName == ((SystemGraphic)obj).GraphicName;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        // -------------------------------------------------------------------
        // CreateCopy
        // -------------------------------------------------------------------

        public SystemGraphic CreateCopy()
        {
            object[] options = null;
            if (Options != null)
            {
                options = new object[Options.Length];
                for (int i = 0; i < Options.Length; i++)
                {
                    options[i] = Options[i];
                }
            }

            return new SystemGraphic(GraphicName, IsRTP, GraphicKind, options);
        }

        // -------------------------------------------------------------------
        // IsNone
        // -------------------------------------------------------------------

        public bool IsNone()
        {
            return GraphicName == WANOK.NONE_IMAGE_STRING;
        }

        // -------------------------------------------------------------------
        // IsTileset
        // -------------------------------------------------------------------

        public bool IsTileset()
        {
            return GraphicName == WANOK.TILESET_IMAGE_STRING;
        }

        // -------------------------------------------------------------------
        // GetGraphicPath
        // -------------------------------------------------------------------

        public string GetGraphicPath()
        {
            if (IsNone()) return null;
            return IsRTP ? GetRTPPath(GraphicName) : GetLocalPath(GraphicName);
        }

        // -------------------------------------------------------------------
        // GetRTPPath
        // -------------------------------------------------------------------

        public string GetRTPPath()
        {
            return Path.Combine(WANOK.Game.System.PathRTP, GetRessourcesPath());
        }

        public string GetRTPPath(string fileName)
        {
            return Path.Combine(GetRTPPath(), fileName);
        }

        // -------------------------------------------------------------------
        // GetLocalPath
        // -------------------------------------------------------------------

        public string GetLocalPath()
        {
            return GetRessourcesPath();
        }

        public string GetLocalPath(string fileName)
        {
            return Path.Combine(GetLocalPath(), fileName);
        }

        // -------------------------------------------------------------------
        // GetRessourcesPath
        // -------------------------------------------------------------------

        public string GetRessourcesPath()
        {
            switch (GraphicKind)
            {
                case GraphicKind.Tileset:
                    return Path.Combine("Content", "Pictures", "Textures2D", "Tilesets");
                case GraphicKind.Autotile:
                    return Path.Combine("Content", "Pictures", "Textures2D", "Autotiles");
                case GraphicKind.Character:
                    return Path.Combine("Content", "Pictures", "Textures2D", "Characters");
                case GraphicKind.Relief:
                    return Path.Combine("Content", "Pictures", "Textures2D", "Reliefs");
                case GraphicKind.Other:
                    return Path.Combine("Content", "Pictures", "UI", "Others");
                case GraphicKind.Icon:
                    return Path.Combine("Content", "Pictures", "UI", "Icons");
                case GraphicKind.Bar:
                    return Path.Combine("Content", "Pictures", "UI", "Bars");
                default:
                    return "";
            }
        }

        // -------------------------------------------------------------------
        // LoadTexture
        // -------------------------------------------------------------------

        public Texture2D LoadTexture(GraphicsDevice device)
        {
            string path = "";
            try
            {
                path = GetGraphicPath();
                if (path == null) return Game1.TexNone;
                using (FileStream stream = new FileStream(path, FileMode.Open, FileAccess.Read))
                {
                    return Texture2D.FromStream(device, stream);
                }
            }
            catch
            {
                WANOK.PrintError("Could not load the image " + path);
                return Game1.TexNone;
            }
        }

        // -------------------------------------------------------------------
        // GetCharacterAct
        // -------------------------------------------------------------------

        public SystemGraphic GetCharacterAct()
        {
            if (IsTileset() || IsNone())
            {
                return this;
            }

            SystemGraphic graphic = CreateCopy();
            graphic.GraphicName = Path.GetFileNameWithoutExtension(graphic.GraphicName) + "_act" + Path.GetExtension(graphic.GraphicName);
            if (File.Exists(graphic.GetGraphicPath())) return graphic;
            else return this;
        }
    }
}
