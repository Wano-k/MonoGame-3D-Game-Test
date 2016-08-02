﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Test;

namespace RPG_Paper_Maker
{
    [Serializable]
    public class Tileset : SuperListItem
    {
        public static int MAX_TILESETS = 9999;
        public SystemGraphic Graphic;
        public Collision Collision;
        public List<int> Autotiles = new List<int>();
        public List<int> Reliefs = new List<int>();


        // -------------------------------------------------------------------
        // Constructors
        // -------------------------------------------------------------------

        public Tileset(int id) : this(id, "", new SystemGraphic(WANOK.NONE_IMAGE_STRING, true, GraphicKind.Tileset), new Collision(), new List<int>(), new List<int>())
        {

        }

        public Tileset(int id, string n, SystemGraphic graphic, Collision collision, List<int> autotiles, List<int> reliefs)
        {
            Id = id;
            Name = n;
            Graphic = graphic;
            Collision = collision;
            Autotiles = autotiles;
            Reliefs = reliefs;
        }

        // -------------------------------------------------------------------
        // CreateCopy
        // -------------------------------------------------------------------

        public override SuperListItem CreateCopy()
        {
            return new Tileset(Id, Name, Graphic.CreateCopy(), Collision.CreateCopy(), new List<int>(Autotiles), new List<int>(Reliefs));
        }
    }
}