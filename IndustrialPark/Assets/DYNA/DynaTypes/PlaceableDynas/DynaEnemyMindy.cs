﻿using HipHopFile;
using System.Collections.Generic;
using System.ComponentModel;

namespace IndustrialPark
{
    public enum EnemyMindyType : uint
    {
        patrick_npc_bind = 0x7362A2AC,
        spongebob_npc_bind = 0xA92E3C8F,
        mindy_shell_bind = 0x5D13D2A4
    }

    public class DynaEnemyMindy : DynaEnemySB
    {
        private const string dynaCategoryName = "Enemy:SB:Mindy";

        protected override short constVersion => 3;

        [Category(dynaCategoryName)]
        public EnemyMindyType MindyType
        {
            get => (EnemyMindyType)(uint)Model;
            set => Model = (uint)value;
        }
        [Category(dynaCategoryName)]
        public AssetID TaskBox1 { get; set; }
        [Category(dynaCategoryName)]
        public AssetSingle ClamOpenDistance { get; set; }
        [Category(dynaCategoryName)]
        public AssetSingle ClamCloseDistance { get; set; }
        [Category(dynaCategoryName)]
        public AssetID TextBox { get; set; }
        [Category(dynaCategoryName)]
        public int UnknownInt60 { get; set; }
        [Category(dynaCategoryName)]
        public AssetID TaskBox2 { get; set; }

        public DynaEnemyMindy(Section_AHDR AHDR, Game game, Endianness endianness) : base(AHDR, DynaType.Enemy__SB__Mindy, game, endianness)
        {
            using (var reader = new EndianBinaryReader(AHDR.data, endianness))
            {
                reader.BaseStream.Position = entityDynaEndPosition;

                TaskBox1 = reader.ReadUInt32();
                ClamOpenDistance = reader.ReadSingle();
                ClamCloseDistance = reader.ReadSingle();
                TextBox = reader.ReadUInt32();
                UnknownInt60 = reader.ReadInt32();
                TaskBox2 = reader.ReadUInt32();
            }
        }

        protected override byte[] SerializeDyna(Game game, Endianness endianness)
        {
            using (var writer = new EndianBinaryWriter(endianness))
            {
                writer.Write(SerializeEntityDyna(endianness));
                writer.Write(TaskBox1);
                writer.Write(ClamOpenDistance);
                writer.Write(ClamCloseDistance);
                writer.Write(TextBox);
                writer.Write(UnknownInt60);
                writer.Write(TaskBox2);

                return writer.ToArray();
            }
        }

        public override void Verify(ref List<string> result)
        {
            base.Verify(ref result);

            Verify(TaskBox1, ref result);
            Verify(TaskBox2, ref result);
            Verify(TextBox, ref result);
        }

        public static bool dontRender = false;
        public override bool DontRender => dontRender;
    }
}