using System.Windows;
using DzFunctions.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace DzFunctions
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void BtnAllOfficeSupplies_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using var db = new FirmaContext();
                var officeSupplies = db.OfficeSupplies
                                       .FromSqlRaw("EXEC GetAllOfficeSupplies")
                                       .AsEnumerable()
                                       .ToList();

                foreach (var supply in officeSupplies)
                {
                    supply.SupplyType = db.SupplyTypes.Find(supply.SupplyTypeID);
                    supply.Sales = db.Sales
                                     .Where(s => s.SupplyID == supply.SupplyID)
                                     .Include(s => s.SalesManager)
                                     .Include(s => s.BuyerCompany)
                                     .ToList();
                }
                var result = officeSupplies.Select(o => new
                {
                    o.SupplyID,
                    o.Name,
                    SupplyType = o.SupplyType?.TypeName,
                    o.Quantity,
                    o.UnitCost,
                    o.Description,
                    SalesCount = o.Sales.Count,
                    SoldTo = o.Sales.Any() ? string.Join(", ", o.Sales.Select(s => s.BuyerCompany.CompanyName)) : "Нет продаж",
                    ManagedBy = o.Sales.Any() ? string.Join(", ", o.Sales.Select(s => s.SalesManager.FirstName + " " + s.SalesManager.LastName)) : "Нет менеджера"
                }).ToList();
                dgData.ItemsSource = result;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void BtnAllTypes_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using var db = new FirmaContext();
                var supplyTypes = db.SupplyTypes
                                    .FromSqlRaw("EXEC GetAllSupplyTypes")
                                    .AsEnumerable()
                                    .Select(t => new
                                    {
                                        t.TypeID,
                                        t.TypeName,
                                        OfficeSupplies = string.Join(", ", db.OfficeSupplies
                                            .Where(os => os.SupplyTypeID == t.TypeID)
                                            .Select(os => os.Name))
                                    }).ToList();
                dgData.ItemsSource = supplyTypes;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

      

        private void BtnAllBuyers_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using var db = new FirmaContext();
                var buyers = db.BuyerCompanies
                               .FromSqlRaw("EXEC GetAllBuyerCompanies")
                               .AsEnumerable()
                               .Select(b => new
                               {
                                   b.BuyerID,
                                   b.CompanyName,
                                   b.Address,
                                   PurchasesCount = db.Sales.Count(s => s.BuyerID == b.BuyerID)
                               }).ToList();
                dgData.ItemsSource = buyers;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void BtnMaxQuantity_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using var db = new FirmaContext();
                var supplies = db.OfficeSupplies
                                 .FromSqlRaw("EXEC GetMaxQuantitySupplies")
                                 .AsEnumerable()
                                 .Select(o => new
                                 {
                                     o.SupplyID,
                                     o.Name,
                                     SupplyType = db.SupplyTypes.Find(o.SupplyTypeID)?.TypeName,
                                     o.Quantity,
                                     o.UnitCost,
                                     o.Description,
                                     SalesCount = db.Sales.Count(s => s.SupplyID == o.SupplyID)
                                 }).ToList();
                dgData.ItemsSource = supplies;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void BtnMinQuantity_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using var db = new FirmaContext();
                var supplies = db.OfficeSupplies
                                 .FromSqlRaw("EXEC GetMinQuantitySupplies")
                                 .AsEnumerable()
                                 .Select(o => new
                                 {
                                     o.SupplyID,
                                     o.Name,
                                     SupplyType = db.SupplyTypes.Find(o.SupplyTypeID)?.TypeName,
                                     o.Quantity,
                                     o.UnitCost,
                                     o.Description,
                                     SalesCount = db.Sales.Count(s => s.SupplyID == o.SupplyID)
                                 }).ToList();
                dgData.ItemsSource = supplies;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void BtnAllManagers_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using var db = new FirmaContext();
                var managers = db.SalesManagers
                                 .FromSqlRaw("EXEC GetAllManagers")
                                 .AsEnumerable()
                                 .Select(m => new
                                 {
                                     m.ManagerID,
                                     FullName = $"{m.FirstName} {m.LastName}",
                                     m.Phone,
                                     SalesCount = db.Sales.Count(s => s.ManagerID == m.ManagerID)
                                 }).ToList();
                dgData.ItemsSource = managers;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void BtnMaxCost_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using var db = new FirmaContext();
                var supplies = db.OfficeSupplies
                                 .FromSqlRaw("EXEC GetMaxUnitCostSupplies")
                                 .AsEnumerable()
                                 .Select(o => new
                                 {
                                     o.SupplyID,
                                     o.Name,
                                     SupplyType = db.SupplyTypes.Find(o.SupplyTypeID)?.TypeName,
                                     o.Quantity,
                                     o.UnitCost,
                                     o.Description,
                                     SalesCount = db.Sales.Count(s => s.SupplyID == o.SupplyID)
                                 }).ToList();
                dgData.ItemsSource = supplies;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void BtnMinCost_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using var db = new FirmaContext();
                var supplies = db.OfficeSupplies
                                 .FromSqlRaw("EXEC GetMinUnitCostSupplies")
                                 .AsEnumerable()
                                 .Select(o => new
                                 {
                                     o.SupplyID,
                                     o.Name,
                                     SupplyType = db.SupplyTypes.Find(o.SupplyTypeID)?.TypeName,
                                     o.Quantity,
                                     o.UnitCost,
                                     o.Description,
                                     SalesCount = db.Sales.Count(s => s.SupplyID == o.SupplyID)
                                 }).ToList();
                dgData.ItemsSource = supplies;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void BtnByManager_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string input = Microsoft.VisualBasic.Interaction.InputBox("Введите ManagerID:", "Поиск", "1");
                if (!int.TryParse(input, out int managerId)) return;
                using var db = new FirmaContext();
                var param = new SqlParameter("@managerId", managerId);
                var officeSupplies = db.OfficeSupplies
                                       .FromSqlRaw("EXEC ShowOfficeSuppliesByManager @managerId", param)
                                       .AsEnumerable()
                                       .Select(o => new
                                       {
                                           o.SupplyID,
                                           o.Name,
                                           SupplyType = db.SupplyTypes.Find(o.SupplyTypeID)?.TypeName,
                                           o.Quantity,
                                           o.UnitCost,
                                           o.Description
                                       }).ToList();
                dgData.ItemsSource = officeSupplies;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void BtnByBuyer_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string input = Microsoft.VisualBasic.Interaction.InputBox("Введите BuyerID:", "Поиск", "1");
                if (!int.TryParse(input, out int buyerId)) return;
                using var db = new FirmaContext();
                var param = new SqlParameter("@buyerId", buyerId);
                var officeSupplies = db.OfficeSupplies
                                       .FromSqlRaw("EXEC ShowOfficeSuppliesByBuyer @buyerId", param)
                                       .AsEnumerable()
                                       .Select(o => new
                                       {
                                           o.SupplyID,
                                           o.Name,
                                           SupplyType = db.SupplyTypes.Find(o.SupplyTypeID)?.TypeName,
                                           o.Quantity,
                                           o.UnitCost,
                                           o.Description
                                       }).ToList();
                dgData.ItemsSource = officeSupplies;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void BtnByType_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string input = Microsoft.VisualBasic.Interaction.InputBox("Введите TypeID:", "Поиск", "1");
                if (!int.TryParse(input, out int typeId)) return;
                using var db = new FirmaContext();
                var param = new SqlParameter("@typeId", typeId);
                var officeSupplies = db.OfficeSupplies
                                       .FromSqlRaw("EXEC ShowOfficeSuppliesByType @typeId", param)
                                       .AsEnumerable()
                                       .Select(o => new
                                       {
                                           o.SupplyID,
                                           o.Name,
                                           SupplyType = db.SupplyTypes.Find(o.SupplyTypeID)?.TypeName,
                                           o.Quantity,
                                           o.UnitCost,
                                           o.Description
                                       }).ToList();
                dgData.ItemsSource = officeSupplies;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void BtnLatestSale_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using var db = new FirmaContext();
                var sales = db.Sales
                              .FromSqlRaw("EXEC GetLatestSale")
                              .AsEnumerable()
                              .Select(s => new
                              {
                                  s.SaleID,
                                  SupplyName = db.OfficeSupplies.Find(s.SupplyID)?.Name,
                                  Manager = db.SalesManagers.Find(s.ManagerID)?.FirstName + " " + db.SalesManagers.Find(s.ManagerID)?.LastName,
                                  Buyer = db.BuyerCompanies.Find(s.BuyerID)?.CompanyName,
                                  s.SaleDate,
                                  s.QuantitySold,
                                  s.SalePrice
                              }).ToList();
                dgData.ItemsSource = sales;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void BtnAverageQuantity_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using var db = new FirmaContext();
                var results = db.Set<AverageQuantityDto>()
                                .FromSqlRaw("EXEC GetAverageQuantityByType")
                                .ToList();
                dgData.ItemsSource = results;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void BtnInsertOfficeSupply_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string name = txtNewSupplyName.Text.Trim();
                int typeId = int.Parse(txtNewSupplyTypeId.Text.Trim());
                int quantity = int.Parse(txtNewSupplyQuantity.Text.Trim());
                decimal cost = decimal.Parse(txtNewSupplyCost.Text.Trim());
                string desc = txtNewSupplyDesc.Text.Trim();
                using var db = new FirmaContext();
                var p1 = new SqlParameter("@Name", name);
                var p2 = new SqlParameter("@SupplyTypeID", typeId);
                var p3 = new SqlParameter("@Quantity", quantity);
                var p4 = new SqlParameter("@UnitCost", cost);
                var p5 = new SqlParameter("@Description", desc);
                db.OfficeSupplies
                  .FromSqlRaw("EXEC InsertOfficeSupply @Name, @SupplyTypeID, @Quantity, @UnitCost, @Description",
                              p1, p2, p3, p4, p5)
                  .ToList();
                MessageBox.Show("Канцтовар добавлен!");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void BtnUpdateOfficeSupply_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int supplyId = int.Parse(txtUpdSupplyID.Text.Trim());
                string name = txtUpdSupplyName.Text.Trim();
                int typeId = int.Parse(txtUpdSupplyTypeId.Text.Trim());
                int quantity = int.Parse(txtUpdSupplyQuantity.Text.Trim());
                decimal cost = decimal.Parse(txtUpdSupplyCost.Text.Trim());
                string desc = txtUpdSupplyDesc.Text.Trim();
                using var db = new FirmaContext();
                var p0 = new SqlParameter("@SupplyID", supplyId);
                var p1 = new SqlParameter("@Name", name);
                var p2 = new SqlParameter("@SupplyTypeID", typeId);
                var p3 = new SqlParameter("@Quantity", quantity);
                var p4 = new SqlParameter("@UnitCost", cost);
                var p5 = new SqlParameter("@Description", desc);
                int rows = db.Database.ExecuteSqlRaw(
                    "EXEC UpdateOfficeSupply @SupplyID, @Name, @SupplyTypeID, @Quantity, @UnitCost, @Description",
                    p0, p1, p2, p3, p4, p5
                );
                MessageBox.Show($"Обновлено записей: {rows}");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void BtnDeleteOfficeSupply_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int supplyId = int.Parse(txtDelSupplyID.Text.Trim());
                using var db = new FirmaContext();
                var p = new SqlParameter("@SupplyID", supplyId);
                int rows = db.Database.ExecuteSqlRaw(
                    "EXEC DeleteOfficeSupply @SupplyID", p);
                MessageBox.Show($"Удалено записей: {rows}");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void BtnInsertSupplyType_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string typeName = txtNewTypeName.Text.Trim();
                if (string.IsNullOrEmpty(typeName))
                {
                    MessageBox.Show("Введите название типа!");
                    return;
                }
                using var db = new FirmaContext();
                var p = new SqlParameter("@TypeName", typeName);
                db.SupplyTypes
                  .FromSqlRaw("EXEC InsertSupplyType @TypeName", p)
                  .ToList();
                MessageBox.Show("Новый тип добавлен!");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void BtnUpdateSupplyType_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int typeId = int.Parse(txtUpdTypeID.Text.Trim());
                string typeName = txtUpdTypeName.Text.Trim();   
                using var db = new FirmaContext();
                var p0 = new SqlParameter("@TypeID", typeId);
                var p1 = new SqlParameter("@TypeName", typeName);
                int rows = db.Database.ExecuteSqlRaw(
                    "EXEC UpdateSupplyType @TypeID, @TypeName", p0, p1);
                MessageBox.Show($"Обновлено записей: {rows}");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void BtnDeleteSupplyType_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int typeId = int.Parse(txtDelTypeID.Text.Trim());
                using var db = new FirmaContext();
                var p = new SqlParameter("@TypeID", typeId);
                int rows = db.Database.ExecuteSqlRaw("EXEC DeleteSupplyType @TypeID", p);
                MessageBox.Show($"Удалено записей: {rows}");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void BtnInsertManager_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string fname = txtNewManagerFName.Text.Trim();
                string lname = txtNewManagerLName.Text.Trim();
                string phone = txtNewManagerPhone.Text.Trim();
                using var db = new FirmaContext();
                var p1 = new SqlParameter("@FirstName", fname);
                var p2 = new SqlParameter("@LastName", lname);
                var p3 = new SqlParameter("@Phone", phone);
                db.SalesManagers
                  .FromSqlRaw("EXEC InsertSalesManager @FirstName, @LastName, @Phone",
                              p1, p2, p3)
                  .ToList();

                MessageBox.Show("Менеджер добавлен!");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void BtnUpdateManager_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int managerId = int.Parse(txtUpdManagerID.Text.Trim());
                string fname = txtUpdManagerFName.Text.Trim();
                string lname = txtUpdManagerLName.Text.Trim();
                string phone = txtUpdManagerPhone.Text.Trim();
                using var db = new FirmaContext();
                var p0 = new SqlParameter("@ManagerID", managerId);
                var p1 = new SqlParameter("@FirstName", fname);
                var p2 = new SqlParameter("@LastName", lname);
                var p3 = new SqlParameter("@Phone", phone);
                int rows = db.Database.ExecuteSqlRaw(
                    "EXEC UpdateSalesManager @ManagerID, @FirstName, @LastName, @Phone",
                     p0, p1, p2, p3);
                MessageBox.Show($"Обновлено записей: {rows}");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void BtnDeleteManager_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int managerId = int.Parse(txtDelManagerID.Text.Trim());
                using var db = new FirmaContext();
                var p = new SqlParameter("@ManagerID", managerId);
                int rows = db.Database.ExecuteSqlRaw("EXEC DeleteSalesManager @ManagerID", p);
                MessageBox.Show($"Удалено записей: {rows}");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void BtnInsertBuyerCompany_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string name = txtNewBuyerName.Text.Trim();
                string addr = txtNewBuyerAddr.Text.Trim();
                using var db = new FirmaContext();
                var p1 = new SqlParameter("@CompanyName", name);
                var p2 = new SqlParameter("@Address", addr);
                db.BuyerCompanies
                  .FromSqlRaw("EXEC InsertBuyerCompany @CompanyName, @Address", p1, p2)
                  .ToList();
                MessageBox.Show("Покупатель добавлен!");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void BtnUpdateBuyerCompany_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int buyerId = int.Parse(txtUpdBuyerID.Text.Trim());
                string name = txtUpdBuyerName.Text.Trim();
                string addr = txtUpdBuyerAddr.Text.Trim();
                using var db = new FirmaContext();
                var p0 = new SqlParameter("@BuyerID", buyerId);
                var p1 = new SqlParameter("@CompanyName", name);
                var p2 = new SqlParameter("@Address", addr);
                int rows = db.Database.ExecuteSqlRaw(
                    "EXEC UpdateBuyerCompany @BuyerID, @CompanyName, @Address",
                     p0, p1, p2);
                MessageBox.Show($"Обновлено записей: {rows}");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void BtnDeleteBuyerCompany_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int buyerId = int.Parse(txtDelBuyerID.Text.Trim());
                using var db = new FirmaContext();
                var p = new SqlParameter("@BuyerID", buyerId);
                int rows = db.Database.ExecuteSqlRaw("EXEC DeleteBuyerCompany @BuyerID", p);
                MessageBox.Show($"Удалено записей: {rows}");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void BtnCallFunctionShowAllSalesManagers_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using var db = new FirmaContext();
                var managers = db.SalesManagers
                                 .FromSqlRaw("SELECT * FROM dbo.ShowAllSalesManagers()")
                                 .AsEnumerable()
                                 .ToList();
                var sales = db.Sales.ToList();
                var officeSupplies = db.OfficeSupplies.ToList();
                var result = managers.Select(m => new
                {
                    m.ManagerID,
                    FullName = $"{m.FirstName} {m.LastName}",
                    m.Phone,
                    Sales = string.Join(", ", sales
                        .Where(s => s.ManagerID == m.ManagerID)
                        .Select(s =>
                        {
                            var supply = officeSupplies.FirstOrDefault(os => os.SupplyID == s.SupplyID);
                            return supply != null ? $"{supply.Name} ({s.QuantitySold} шт.)" : "Неизвестный товар";
                        }))
                }).ToList();
                dgData.ItemsSource = result;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}