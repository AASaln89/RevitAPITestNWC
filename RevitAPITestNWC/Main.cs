using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RevitAPITestNWC
{
    [Transaction(TransactionMode.Manual)]
    public class Main : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            Document doc = commandData.Application.ActiveUIDocument.Document;

            using (var ts = new Transaction(doc, "export ifc"))
            {
                ts.Start();

                ViewPlan view3D = new FilteredElementCollector(doc, View3D)
                                    .OfClass(typeof(View3D))
                                    .Cast<View3D>()
                                    .FirstOrDefault(v => v.ViewType == ViewType.ThreeD);
                var NWCOption = new NavisworksExportOptions();
                doc.Export(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "export.nwc",
                    new List<ElementId> { view3D.Id }, { NWCOption.ViewId});
                ts.Commit();
            }

            return Result.Succeeded;
        }
    }
