﻿using HipHopFile;
using System.Collections.Generic;
using System.ComponentModel;

namespace IndustrialPark
{
    public class DynaGObjectFlythrough : AssetDYNA
    {
        protected override short constVersion => 1;

        [Category("game_object:Flythrough")]
        public AssetID Flythrough { get; set; }

        public DynaGObjectFlythrough(string assetName) : base(assetName, DynaType.game_object__Flythrough, 1)
        {
            Flythrough = 0;
        }

        public DynaGObjectFlythrough(Section_AHDR AHDR, Game game, Endianness endianness) : base(AHDR, DynaType.game_object__Flythrough, game, endianness)
        {
            using (var reader = new EndianBinaryReader(AHDR.data, endianness))
            {
                reader.BaseStream.Position = dynaDataStartPosition;
                Flythrough = reader.ReadUInt32();
            }
        }

        protected override byte[] SerializeDyna(Game game, Endianness endianness)
        {
            using (var writer = new EndianBinaryWriter(endianness))
            {
                writer.Write(Flythrough);
                return writer.ToArray();
            }
        }

        public override void Verify(ref List<string> result)
        {
            if (Flythrough == 0)
                result.Add("Flythrough with no FLY reference");
            Verify(Flythrough, ref result);

            base.Verify(ref result);
        }
    }
}