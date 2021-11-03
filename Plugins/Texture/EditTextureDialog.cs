using System;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using AssetsAdvancedEditor.Utils;
using UnityTools;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using Image = SixLabors.ImageSharp.Image;

namespace Plugins.Texture
{
    public partial class EditTextureDialog : Form
    {
        private TextureFile tex { get; }
        private AssetTypeValueField texField { get; }
        private byte[] imgBytes;
        public EditTextureDialog(TextureFile tex, AssetTypeValueField texField)
        {
            InitializeComponent();
            this.tex = tex;
            this.texField = texField;
            imgBytes = null;

            var formats = Enum.GetValues(typeof(TextureFormat));
            foreach (var format in formats)
            {
                cboxTextureFormat.Items.Add(format);
            }

            tboxTextureName.Text = tex.m_Name;
            cboxTextureFormat.SelectedIndex = tex.m_TextureFormat - 1;
            chboxHasMipMaps.Checked = tex.m_MipMap;
            chboxIsReadable.Checked = tex.m_IsReadable;
            cboxFilterMode.SelectedIndex = tex.m_TextureSettings.m_FilterMode;
            tboxAniso.Text = tex.m_TextureSettings.m_Aniso.ToString();
            tboxMipMapBias.Text = tex.m_TextureSettings.m_MipBias.ToString(CultureInfo.CurrentCulture);
            cboxWrapU.SelectedIndex = tex.m_TextureSettings.m_WrapU;
            cboxWrapV.SelectedIndex = tex.m_TextureSettings.m_WrapV;
            tboxLightmapFormat.Text = tex.m_LightmapFormat.ToString();
            cboxColorSpace.SelectedIndex = tex.m_ColorSpace;
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            var ofd = new OpenFileDialog
            {
                Title = @"Open texture",
                Filter = @"PNG file (*.png)|*.png|TGA file (*.tga)|*.tga|All types (*.*)|*.*"
            };
            if (ofd.ShowDialog() != DialogResult.OK) return;
            try
            {
                using var image = Image.Load<Rgba32>(ofd.FileName);
                tex.m_Width = image.Width;
                tex.m_Height = image.Height;

                image.Mutate(i => i.Flip(FlipMode.Vertical));
                imgBytes = image.TryGetSinglePixelSpan(out var pixelSpan)
                    ? MemoryMarshal.AsBytes(pixelSpan).ToArray()
                    : null;
                if (imgBytes == null)
                {
                    MsgBoxUtils.ShowErrorDialog(this, "Failed to parse current texture.");
                }
            }
            catch (Exception ex)
            {
                MsgBoxUtils.ShowErrorDialog(this, "Something went wrong when importing the texture:\n" + ex);
            }
        }

        private void btnView_Click(object sender, EventArgs e)
        {
            // todo
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (imgBytes == null)
            {
                DialogResult = DialogResult.Cancel;
            }

            var format = (TextureFormat)(cboxTextureFormat.SelectedIndex + 1);
            var encodedBytes = TextureManager.EncodeTexture(imgBytes, tex.m_Width, tex.m_Height, format);

            var m_StreamData = texField.Get("m_StreamData");
            m_StreamData.Get("offset").Value.Set(0);
            m_StreamData.Get("size").Value.Set(0);
            m_StreamData.Get("path").Value.Set("");

            var image_data = texField.Get("image data");
            image_data.Value.type = EnumValueTypes.ByteArray;
            image_data.TemplateField.valueType = EnumValueTypes.ByteArray;
            var byteArray = new AssetTypeByteArray
            {
                size = (uint)encodedBytes.Length,
                data = encodedBytes
            };
            image_data.Value.Set(byteArray);

            texField.Get("m_ColorSpace").Value.Set(cboxColorSpace.SelectedIndex);
            texField.Get("m_LightmapFormat").Value.Set(tboxLightmapFormat.Text);

            var m_TextureSettings = texField.Get("m_TextureSettings");
            m_TextureSettings.Get("m_FilterMode").Value.Set(cboxFilterMode.SelectedIndex);
            m_TextureSettings.Get("m_Aniso").Value.Set(tboxAniso.Text);
            m_TextureSettings.Get("m_MipBias").Value.Set(tboxMipMapBias.Text);
            m_TextureSettings.Get("m_WrapU").Value.Set(cboxWrapU.SelectedIndex);
            m_TextureSettings.Get("m_WrapV").Value.Set(cboxWrapV.SelectedIndex);
            m_TextureSettings.Get("m_WrapW").Value.Set(1);

            texField.Get("m_TextureDimension").Value.Set(2);

            texField.Get("m_ImageCount").Value.Set(1);

            texField.Get("m_IsReadable").Value.Set(false);

            texField.Get("m_MipCount").Value.Set(1);

            texField.Get("m_TextureFormat").Value.Set((int)format);

            texField.Get("m_CompleteImageSize").Value.Set(encodedBytes.Length);

            texField.Get("m_Width").Value.Set(tex.m_Width);
            texField.Get("m_Height").Value.Set(tex.m_Height);

            texField.Get("m_Name").Value.Set(tboxTextureName.Text);
        }
    }
}
