﻿using HipHopFile;
using System.Collections.Generic;
using System.ComponentModel;

namespace IndustrialPark
{
    public enum DynaRingControlPlayerType
    {
        Drive = 0,
        SpongebobPatrick = 1,
        Spongeball = 2,
        Unknown3 = 3,
        Slide = 4,
        SonicWaveGuitar = 5
    }

    public class DynaGObjectRingControl : AssetDYNA
    {
        private const string dynaCategoryName = "game_object:RingControl";

        protected override short constVersion => 3;

        public static uint RingModelAssetID = 0;

        [Category(dynaCategoryName)]
        public DynaRingControlPlayerType PlayerType { get; set; }
        private uint _ringModel;
        [Category(dynaCategoryName)]
        public AssetID RingModel
        {
            get => _ringModel;
            set
            {
                _ringModel = value;
                RingModelAssetID = value;
            }
        }
        [Category(dynaCategoryName)]
        public AssetSingle DefaultWarningTime { get; set; }
        [Category(dynaCategoryName)]
        public int UnusedOffset { get; set; }
        [Category(dynaCategoryName)]
        public AssetID RingSoundGroup1 { get; set; }
        [Category(dynaCategoryName)]
        public AssetID RingSoundGroup2 { get; set; }
        [Category(dynaCategoryName)]
        public AssetID RingSoundGroup3 { get; set; }
        [Category(dynaCategoryName)]
        public AssetID RingSoundGroup4 { get; set; }
        [Category(dynaCategoryName)]
        public int NumNextRingsToShow { get; set; }
        [Category(dynaCategoryName)]
        public AssetID[] Rings { get; set; }

        public DynaGObjectRingControl(string assetName) : base(assetName, DynaType.game_object__RingControl, 3)
        {
            RingModel = "test_ring";
            UnusedOffset = 40;
            RingSoundGroup1 = "RING_SGRP";
            RingSoundGroup2 = 0;
            RingSoundGroup3 = 0;
            RingSoundGroup4 = 0;
            NumNextRingsToShow = 1;
            Rings = new AssetID[0];
        }

        public DynaGObjectRingControl(Section_AHDR AHDR, Game game, Endianness endianness) : base(AHDR, DynaType.game_object__RingControl, game, endianness)
        {
            using (var reader = new EndianBinaryReader(AHDR.data, endianness))
            {
                reader.BaseStream.Position = dynaDataStartPosition;

                PlayerType = (DynaRingControlPlayerType)reader.ReadInt32();
                RingModel = reader.ReadUInt32();
                DefaultWarningTime = reader.ReadSingle();
                int ringCount = reader.ReadInt32();
                UnusedOffset = reader.ReadInt32();
                RingSoundGroup1 = reader.ReadUInt32();
                RingSoundGroup2 = reader.ReadUInt32();
                RingSoundGroup3 = reader.ReadUInt32();
                RingSoundGroup4 = reader.ReadUInt32();
                NumNextRingsToShow = reader.ReadInt32();
                Rings = new AssetID[ringCount];
                for (int i = 0; i < Rings.Length; i++)
                    Rings[i] = reader.ReadUInt32();
            }
        }

        protected override byte[] SerializeDyna(Game game, Endianness endianness)
        {
            using (var writer = new EndianBinaryWriter(endianness))
            {
                writer.Write((int)PlayerType);
                writer.Write(RingModel);
                writer.Write(DefaultWarningTime);
                writer.Write(Rings.Length);
                writer.Write(UnusedOffset);
                writer.Write(RingSoundGroup1);
                writer.Write(RingSoundGroup2);
                writer.Write(RingSoundGroup3);
                writer.Write(RingSoundGroup4);
                writer.Write(NumNextRingsToShow);
                foreach (var i in Rings)
                    writer.Write(i);

                return writer.ToArray();
            }
        }

        public override void Verify(ref List<string> result)
        {
            if (RingModel == 0)
                result.Add("Ring Control with no Ring Model reference");
            Verify(RingModel, ref result);

            if (RingSoundGroup1 == 0)
                result.Add("Ring Control with no SGRP reference");
            Verify(RingSoundGroup1, ref result);

            foreach (var ring in Rings)
                Verify(ring, ref result);

            base.Verify(ref result);
        }
    }
}