﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Linq;
using System.IO;
using Audiosurf_SkinChanger.Engine;
using Audiosurf_SkinChanger.Utilities;

namespace Audiosurf_SkinChanger.Skin_Creator
{
    public partial class SkinCreatorForm : Form
    {
        public event Action OnSkinExprotrted;

        private enum States
        {
            Idle,
            Active
        }

        private Dictionary<string, PictureBox> assotiateTable;
        private Dictionary<string, Dictionary<States, Bitmap>> statesTable;
        private Dictionary<string, ImageInfo> imageAssociationTable;
        private Dictionary<PictureBox, ImageGroup> skinImageGroupAssociationTable;
        private Dictionary<PictureBox, NamedBitmap> skinBitmapsAssociationTable;
        private Dictionary<string, Size> sizesAssociationTalbe;
        private PictureBox[] tilesGroup;
        private AudiosurfSkin skin;
        private SkinPackager skinPackager;

        private string[] SizesStrings = new[] { "64x64", "128x128", "256x256", "512x512" };

        public SkinCreatorForm()
        {
            InitializeComponent();
            LoadIdleImages();
            CreateAssotiativeTable();
            CreateStateAssotiatieTable();
            CreateImageAssociationTable();
            CreateSizesAssociationTable();
            skin = new AudiosurfSkin();
            CreateSkinFieldsAssotiationTables();
            tilesGroup = new[] { tile1, tile2, tile3, tile4 };

            tilesetSizes.Items.AddRange(SizesStrings);
            hitsSizes.Items.AddRange(SizesStrings);
            particlesSizes.Items.AddRange(SizesStrings);
            ringsSizes.Items.AddRange(SizesStrings);

            tilesetSizes.SelectedIndex = 0;
            hitsSizes.SelectedIndex = 0;
            particlesSizes.SelectedIndex = 0;
            ringsSizes.SelectedIndex = 0;

            skinPackager = new SkinPackager();
            isRescaleCheckButton.Checked = true;
            skinNameEntry.Text = "Unnamed skin";
        }

        private void CreateSizesAssociationTable()
        {
            sizesAssociationTalbe = new Dictionary<string, Size>()
            {
                {SizesStrings[0], new Size(64,64) },
                {SizesStrings[1], new Size(128,128) },
                {SizesStrings[2], new Size(256,256) },
                {SizesStrings[3], new Size(512, 512) }
            };
        }

        private void CreateAssotiativeTable()
        {
            assotiateTable = new Dictionary<string, PictureBox>()
            {
                { "Sphere1", Sphere1 },
                { "Sphere2", Sphere2 },
                { "Sphere3", Sphere3 },
                { "tile1", tile1 },
                {"tile2", tile2 },
                {"tile3", tile3 },
                {"tile4", tile4 },
                {"tileflyup", tileflyup },
                {"part1", part1},
                {"part2", part2 },
                {"part3", part3 },
                {"ring1", ring1 },
                {"ring2", ring2 },
                {"ring3", ring3 },
                {"ring4", ring4 },
                {"hit1", hit1 },
                {"hit2", hit2 }
            };
        }

        private void CreateStateAssotiatieTable()
        {
            statesTable = new Dictionary<string, Dictionary<States, Bitmap>>
            {
                { "Sphere1", new Dictionary<States, Bitmap>() { {States.Active, Images.Add_SkySphere_Active }, { States.Idle, Images.Add_Skysphere} } },
                { "Sphere2", new Dictionary<States, Bitmap>() { {States.Active, Images.Add_SkySphere_Active }, { States.Idle, Images.Add_Skysphere} } },
                { "Sphere3", new Dictionary<States, Bitmap>() { {States.Active, Images.Add_SkySphere_Active }, { States.Idle, Images.Add_Skysphere} } },
                { "tile1", new Dictionary<States, Bitmap>() { { States.Active, Images.Add_Tiles_Active }, { States.Idle, Images.Add_Tiles} } },
                {"tile2", new Dictionary<States, Bitmap>() { { States.Active, Images.Add_Tiles_Active }, { States.Idle, Images.Add_Tiles } } },
                {"tile3", new Dictionary<States, Bitmap>() { { States.Active, Images.Add_Tiles_Active }, { States.Idle, Images.Add_Tiles } } },
                {"tile4", new Dictionary<States, Bitmap>() { { States.Active, Images.Add_Tiles_Active }, { States.Idle, Images.Add_Tiles } } },
                {"tileflyup", new Dictionary<States, Bitmap>() { { States.Active, Images.Add_Tile_Flyup_Active }, { States.Idle, Images.Add_TileFlyup } } },
                {"part1", new Dictionary<States, Bitmap>() { { States.Active, Images.Add_Particles1_Active }, { States.Idle, Images.Add_Particles1 } } },
                {"part2", new Dictionary<States, Bitmap>() { { States.Active, Images.Add_Particles2_Active }, { States.Idle, Images.Add_Particles2 } } },
                {"part3", new Dictionary<States, Bitmap>() { { States.Active, Images.Add_Particles3_Active }, { States.Idle, Images.Add_Particles3 } } },
                {"ring1", new Dictionary<States, Bitmap>() { { States.Active, Images.Add_Ring1A_Active }, { States.Idle, Images.Add_Ring1A } } },
                {"ring2", new Dictionary<States, Bitmap>() { { States.Active, Images.Add_Ring2A_Active }, { States.Idle, Images.Add_Ring2A } } },
                {"ring3", new Dictionary<States, Bitmap>() { { States.Active, Images.Add_Ring1B_Active }, { States.Idle, Images.Add_Ring1B } } },
                {"ring4", new Dictionary<States, Bitmap>() { { States.Active, Images.Add_Ring2B_Active }, { States.Idle, Images.Add_Ring2B } } },
                {"hit1", new Dictionary<States, Bitmap>() { { States.Active, Images.Add_Hit1_Active }, { States.Idle, Images.Add_Hit1 } } },
                {"hit2", new Dictionary<States, Bitmap>() { { States.Active, Images.Add_Hit2_Active }, { States.Idle, Images.Add_Hit2 } } }
            };
        }

        private void CreateImageAssociationTable()
        {
            imageAssociationTable = new Dictionary<string, ImageInfo>()
            {
                { "Sphere1", new ImageInfo("png", "Skysphere_White.png") },
                { "Sphere2", new ImageInfo("png", "Skysphere_Black.png") },
                { "Sphere3", new ImageInfo("png", "Skysphere_Grey.png") },
                { "tile1", new ImageInfo("png", "tiles.png") },
                {"tile2", new ImageInfo("png", "tiles.png") },
                {"tile3", new ImageInfo("png", "tiles.png") },
                {"tile4", new ImageInfo("png", "tiles.png") },
                {"tileflyup", new ImageInfo("png", "tileflyup.png") },
                {"part1", new ImageInfo("png", "particles1.png")},
                {"part2", new ImageInfo("jpg", "particles2.jpg") },
                {"part3", new ImageInfo("jpg", "particles3.jpg") },
                {"ring1", new ImageInfo("png", "ring1A.png") },
                {"ring2", new ImageInfo("jpg", "ring2A.jpg") },
                {"ring3", new ImageInfo("png", "ring1B.png") },
                {"ring4", new ImageInfo("jpg", "ring2B.jpg") },
                {"hit1", new ImageInfo("png", "hit1.png") },
                {"hit2", new ImageInfo("jpg", "hit2.jpg") }
            };
        }

        private void CreateSkinFieldsAssotiationTables()
        {
            skinBitmapsAssociationTable = new Dictionary<PictureBox, NamedBitmap>()
            {
                {tile1, skin.Tiles },
                {tile2, skin.Tiles },
                {tile3, skin.Tiles },
                {tile4, skin.Tiles },

                {tileflyup, skin.TilesFlyup }
            };


            skinImageGroupAssociationTable = new Dictionary<PictureBox, ImageGroup>()
            {
                {Sphere1, skin.SkySpheres },
                {Sphere2, skin.SkySpheres },
                {Sphere3, skin.SkySpheres },

                {part1, skin.Particles },
                {part2, skin.Particles },
                {part3, skin.Particles },

                {ring1, skin.Rings },
                {ring2, skin.Rings },
                {ring3, skin.Rings },
                {ring4, skin.Rings },

                {hit1, skin.Hits },
                {hit2, skin.Hits },
            };
        }


        private void LoadIdleImages()
        {
            Sphere1.Image = Images.Add_Skysphere;
            Sphere2.Image = Images.Add_Skysphere;
            Sphere3.Image = Images.Add_Skysphere;

            tile1.Image = Images.Add_Tiles;
            tile2.Image = Images.Add_Tiles;
            tile3.Image = Images.Add_Tiles;
            tile4.Image = Images.Add_Tiles;

            tileflyup.Image = Images.Add_TileFlyup;

            part1.Image = Images.Add_Particles1;
            part2.Image = Images.Add_Particles2;
            part3.Image = Images.Add_Particles3;

            ring1.Image = Images.Add_Ring1A;
            ring2.Image = Images.Add_Ring2A;
            ring3.Image = Images.Add_Ring1B;
            ring4.Image = Images.Add_Ring2B;

            hit1.Image = Images.Add_Hit1;
            hit2.Image = Images.Add_Hit2;
        }

        private void SetActiveSphere(object sender, EventArgs e)
        {
            var knownSender = sender as PictureBox;
            assotiateTable[knownSender.Name].Image = statesTable[knownSender.Name][States.Active];
        }

        private void Sphere1_MouseLeave(object sender, EventArgs e)
        {
            var knownSender = sender as PictureBox;
            assotiateTable[knownSender.Name].Image = statesTable[knownSender.Name][States.Idle];
        }

        private void FillImageSlot(object sender, EventArgs e)
        {
            var knownSender = sender as PictureBox;
            if (knownSender == null)
                MessageBox.Show("Oops! Something goes wrong... \nException message doesnt matter, its just strange internal error", "Internal error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                var info = imageAssociationTable[knownSender.Name];
                var bmp = ReadImage(openFileDialog.FileName, info);
                if (bmp == null)
                    return;
                skinImageGroupAssociationTable[knownSender].SetImageByName(info.FileName, (Bitmap)bmp);
                RemoveMouseActions(knownSender);
                knownSender.Image = ((Bitmap)bmp).Rescale(knownSender.Size);
            }
        }

        private NamedBitmap ReadImage(string path, ImageInfo info)
        {
            var ext = Path.GetExtension(openFileDialog.FileName).Replace(".", "");
            if (ext != info.Format)
            {
                MessageBox.Show($"Ooops! Selected texture requiers image format {info.Format}, but you selected .{ext} image!\n", "Image format Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return null;
            }
            return new NamedBitmap(Image.FromFile(openFileDialog.FileName), info);
        }

        private void FillImageSlotNB(object sender, EventArgs e)
        {
            var knownSender = sender as PictureBox;
            if (knownSender == null)
                MessageBox.Show("Oops! Something goes wrong... \nException message doesnt matter, its just strange internal error", "Internal error", MessageBoxButtons.OK, MessageBoxIcon.Error);

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                var info = imageAssociationTable[knownSender.Name];
                var bmp = ReadImage(openFileDialog.FileName, info);
                if (bmp == null)
                    return;
                skinBitmapsAssociationTable[knownSender].SetImage(bmp);
                RemoveMouseActions(knownSender);
                knownSender.Image = ((Bitmap)bmp).Rescale(knownSender.Size);
            }
        }

        private void RemoveMouseActions(PictureBox knownSender)
        {
            knownSender.MouseEnter -= SetActiveSphere;
            knownSender.MouseLeave -= Sphere1_MouseLeave;
        }

        private void SetMouseActions(PictureBox knownSender)
        {
            knownSender.MouseEnter += SetActiveSphere;
            knownSender.MouseLeave += Sphere1_MouseLeave;
        }

        private void AddTileSpritesheet(object sender, EventArgs e)
        {
            var knownSender = sender as PictureBox;
            if (knownSender == null)
                MessageBox.Show("Oops! Something goes wrong... \nException message doesnt matter, its just strange internal error", "Internal error", MessageBoxButtons.OK, MessageBoxIcon.Error);

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                var info = imageAssociationTable[knownSender.Name];
                var bmp = ReadImage(openFileDialog.FileName, info);
                if (bmp == null)
                    return;
                skinBitmapsAssociationTable[knownSender].SetImage(bmp);
                tilesGroup.ForEach(x => RemoveMouseActions(x));
                Bitmap[] splittedSpritesheet = ((Bitmap)bmp).Squarify().Select(x => x.Rescale(knownSender.Size)).ToArray();
                FillTilesGroup(splittedSpritesheet);
            }
        }

        private void FillTilesGroup(Bitmap[] src)
        {
            var indexer = 0;
            var srcEnumerator = src.GetEnumerator();

            while (srcEnumerator.MoveNext())
            {
                tilesGroup[indexer++].Image = (Bitmap)srcEnumerator.Current;
                if (indexer == tilesGroup.Length) return;
            }
        }

        private void ExportSkin(object sender, EventArgs e)
        {
            if (!isRescaleCheckButton.Checked)
            {
                RescaleSkinTextures();
            }
            skin.Name = skinNameEntry.Text;
            skinPackager.CompileTo(skin, "Skins");
            MessageBox.Show("Done!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            OnSkinExprotrted?.Invoke();
        }

        private void ExportSkinTo(object sender, EventArgs e)
        {
            if (!isRescaleCheckButton.Checked)
            {
                RescaleSkinTextures();
            }
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                var path = folderBrowserDialog1.SelectedPath;
                skin.Name = skinNameEntry.Text;
                skinPackager.CompileTo(skin, path);
                MessageBox.Show("Done!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                OnSkinExprotrted?.Invoke();
            }
        }

        private void RescaleSkinTextures()
        {
            skin.Tiles.Apply(x => x?.Rescale(sizesAssociationTalbe[tilesetSizes.Text]));
            skin.TilesFlyup.Apply(x => x?.Rescale(sizesAssociationTalbe[tilesetSizes.Text]));
            skin.Hits.Apply(x => x?.Apply(bmp => bmp?.Rescale(sizesAssociationTalbe[hitsSizes.Text])));
            skin.Rings.Apply(x => x?.Apply(bmp => bmp?.Rescale(sizesAssociationTalbe[ringsSizes.Text])));
            skin.Particles.Apply(x => x?.Apply(bmp => bmp?.Rescale(sizesAssociationTalbe[particlesSizes.Text])));
        }
    }
}
