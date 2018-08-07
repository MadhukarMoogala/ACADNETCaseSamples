using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.GraphicsInterface;
using Autodesk.AutoCAD.Runtime;
using Autodesk.Windows;

namespace RibbonToolTip_Case
{
    public class MyCommands
    {
        public static readonly string TAB_ID = "TAB_ID-4C724C16684D46998A6E5E6810059AE3";
        [CommandMethod("TestRibbonToolTip")]
        public static void Test()
        {
            Autodesk.Windows.RibbonControl rbnCtrl = Autodesk.AutoCAD.Ribbon.RibbonServices.RibbonPaletteSet.RibbonControl;

            //Add custom ribbon tab 
            RibbonTab rbnTab = new RibbonTab
            {
                Title = "Custom commands",
                Id = TAB_ID
            };
            rbnCtrl.Tabs.Add(rbnTab);
            rbnTab.IsActive = true;
            //Add custom ribbon panel
            RibbonPanel rbnPnl = new RibbonPanel
            {
                //Add ribbon panel source
                Source = new RibbonPanelSource
                {
                    Title = "Custom Panel"
                }
            };
            rbnTab.Panels.Add(rbnPnl);
            //Add custom ribbon button 

            Autodesk.Windows.RibbonButton rbnBtn = new RibbonButton
            {
                Text = "ADN",
                CommandParameter = "ADN ",
                ShowText = true,
                Image = GetBitmap("RibbonToolTip_Case.Yoda.jpg", 16, 16),
                LargeImage = GetBitmap("RibbonToolTip_Case.Yoda.jpg", 32, 32),
                ShowImage = true
            };
            Autodesk.Windows.RibbonToolTip rbnTT = new RibbonToolTip
            {
                Command = "ADN",
                Title = "Hello ADN",
                Content = "blah ..",
                ExpandedContent = "Expanded Blah ..."
            };
            rbnBtn.ToolTip = rbnTT;
            rbnPnl.Source.Items.Add(rbnBtn);
        }

        private static ImageSource GetBitmap(string imageName, int Height, int Width)
        {
            BitmapImage image = new BitmapImage();
            image.BeginInit();
            image.StreamSource = Assembly.GetExecutingAssembly().GetManifestResourceStream(imageName);
            image.DecodePixelHeight = Height;
            image.DecodePixelWidth = Width;
            image.EndInit();
            return image;
        }

        public static void Test1()
        {
            Database database = HostApplicationServices.WorkingDatabase;
            Document activeDoc = Application.DocumentManager.MdiActiveDocument;
            Database db = activeDoc.Database;
            Editor ed = activeDoc.Editor;

            PromptEntityResult oRes = ed.GetEntity("Select Entity");
            using (Transaction transaction = database.TransactionManager.StartTransaction())
            {
                Face oFace = transaction.GetObject(oRes.ObjectId, OpenMode.ForRead) as Face;
                if (oFace != null)
                {

                    string strMat = oFace.Material;

                    ObjectId nMat = oFace.MaterialId;
                    //Here change Entity MaterialId to ByBlock

                    DBDictionary matLib = transaction.GetObject(
                           db.MaterialDictionaryId,
                           OpenMode.ForRead
                       ) as DBDictionary;
                    if(matLib.Contains("EngineTurned"))
                    {
                        ObjectId MaterialId = matLib.GetAt("EngineTurned");
                        Material mat = transaction.GetObject(MaterialId, OpenMode.ForRead) as Material;


                    }
                 


                    Material material = transaction.GetObject(nMat, OpenMode.ForRead) as Material;
                    

                    MaterialMap map = material.Diffuse.Map;
                    ImageFileTexture ift = map.Texture as ImageFileTexture;
                    ift.SourceFileName = "";
                }
            }
        }
    }
}
