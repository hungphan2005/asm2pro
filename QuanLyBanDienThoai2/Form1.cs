using Microsoft.EntityFrameworkCore;

namespace QuanLyBanDienThoai2
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            this.Load += Form1_Load;
        }

        private void btnadd_Click(object sender, EventArgs e)
        {
            using (var context = new ProductDBContext())
            {
                var selectedCategory = (int)cmbcategory.SelectedValue;
                var product = new Product
                {
                    Name = txtname.Text,
                    Description = txtdescription.Text,
                    Quantity = int.Parse(txtquantity.Text),
                    Price = double.Parse(txtprice.Text),
                    CategoryId = selectedCategory,
                };
                context.Products.Add(product);
                context.SaveChanges();
                MessageBox.Show("Thêm thành công");
                LoadData();

            }
        }
        private void LoadData()
        {
            using (var context = new ProductDBContext())
            {
                var products = context.Products
                    .Include(p => p.Category)
                    .Select(p => new
                    {
                        p.Id,
                        p.Name,
                        p.Description,
                        p.Quantity,
                        p.Price,
                        p.CategoryId,
                        CategoryName = p.Category.CategoryName
                    }).ToList();
                dataGridView1.DataSource = products;

            }
        }


        private void Form1_Load(object sender, EventArgs e)
        {
            using (var context = new ProductDBContext())
            {
                //Lấy tất cả các category từ cơ sở dữ liệu
                var Categories = context.Categories.ToList();
                // Gán danh sachs category vào combobox
                cmbcategory.DataSource = Categories;
                cmbcategory.DisplayMember = "Name"; //tên để hiển thị
                cmbcategory.ValueMember = "Id"; //Giá trị chính của Id trong bảng category
            }
            LoadCategoryData();
            LoadData();
            LoadUserData();
            LoadInvoices();


        }

        private void btnupdate_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageBox.Show("Vui lòng chọn một sản phẩm cần sửa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Lấy ProductID từ hàng được chọn
            var selectedRow = dataGridView1.SelectedRows[0];
            if (!int.TryParse(selectedRow.Cells["Id"].Value?.ToString(), out int productId))
            {
                MessageBox.Show("Không thể lấy ID sản phẩm để sửa!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                using (var context = new ProductDBContext())
                {
                    // Tìm sản phẩm trong cơ sở dữ liệu
                    var product = context.Products.FirstOrDefault(p => p.Id == productId);
                    if (product != null)
                    {
                        // Kiểm tra và cập nhật từng trường
                        if (string.IsNullOrWhiteSpace(txtname.Text))
                        {
                            MessageBox.Show("Vui lòng nhập tên sản phẩm!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                        product.Name = txtname.Text;

                        product.Description = txtdescription.Text;

                        if (!int.TryParse(txtquantity.Text, out int quantity) || quantity < 0)
                        {
                            MessageBox.Show("Vui lòng nhập số lượng hợp lệ!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                        product.Quantity = quantity;

                        if (!double.TryParse(txtprice.Text, out double price) || price <= 0)
                        {
                            MessageBox.Show("Vui lòng nhập giá hợp lệ!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                        product.Price = price;

                        if (cmbcategory.SelectedValue == null)
                        {
                            MessageBox.Show("Vui lòng chọn danh mục sản phẩm!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                        product.CategoryId = (int)cmbcategory.SelectedValue;

                        // Lưu thay đổi vào cơ sở dữ liệu
                        context.SaveChanges();
                        MessageBox.Show("Sản phẩm đã được cập nhật thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        // Tải lại dữ liệu
                        LoadData();
                    }
                    else
                    {
                        MessageBox.Show("Không tìm thấy sản phẩm trong cơ sở dữ liệu!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Đã xảy ra lỗi khi cập nhật sản phẩm: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }





        private void btndelete_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageBox.Show("Vui lòng chọn một sản phẩm cần xóa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Lấy ProductID từ hàng được chọn, đảm bảo cột có tên chính xác
            var selectedRow = dataGridView1.SelectedRows[0];

            // Kiểm tra tên cột chính xác, thay 'ProductID' nếu cần
            if (!int.TryParse(selectedRow.Cells["id"].Value?.ToString(), out int productId))
            {
                MessageBox.Show("Không thể lấy ID của sản phẩm để xóa!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                // Xác nhận trước khi xóa
                var confirmResult = MessageBox.Show("Bạn có chắc chắn muốn xóa sản phẩm này?",
                                                    "Xác nhận xóa",
                                                    MessageBoxButtons.YesNo,
                                                    MessageBoxIcon.Question);
                if (confirmResult == DialogResult.Yes)
                {
                    using (var context = new ProductDBContext())
                    {
                        var product = context.Products.FirstOrDefault(p => p.Id == productId);
                        if (product != null)
                        {
                            // Xóa sản phẩm
                            context.Products.Remove(product);
                            context.SaveChanges();
                            MessageBox.Show("Xóa sản phẩm thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                            // Tải lại dữ liệu
                            LoadData();
                        }
                        else
                        {
                            MessageBox.Show("Sản phẩm không tồn tại trong cơ sở dữ liệu!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Đã xảy ra lỗi khi xóa sản phẩm: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnsearch_Click(object sender, EventArgs e)
        {
            using (var context = new ProductDBContext())
            {
                var products = context.Products.Where(p => p.Name.Contains(txtsearch.Text)).ToList();
                dataGridView1.DataSource = products;

            }
        }

        private void cmbcategory_DropDown(object sender, EventArgs e)
        {

        }

        private void btnaddcategory_Click(object sender, EventArgs e)
        {
            using (var context = new ProductDBContext())
            {
                var category = new Category
                {
                    CategoryName = txtcatname.Text,
                };
                context.Categories.Add(category);
                int ret = context.SaveChanges();
                if (ret != 0)
                {
                    MessageBox.Show("Them moi du lieu thanh cong ");
                    LoadCategoryData(); // Tải lại danh sách Category


                }
                else
                {
                    MessageBox.Show("Có lỗi xảy ra khi thêm dữ liệu.");
                }
            }
        }
        private void LoadCategoryData()
        {
            using (var context = new ProductDBContext())
            {
                // Lấy danh sách Category từ database
                var categories = context.Categories
                    .Select(c => new
                    {
                        c.Id,
                        c.CategoryName
                    })
                    .ToList();

                // Gán dữ liệu vào DataGridView
                dataGridViewCategory.DataSource = categories;

                cmbcategory.DataSource = categories;
                cmbcategory.DisplayMember = "CategoryName"; // Hiển thị tên danh mục
                cmbcategory.ValueMember = "Id";            // Lưu ID danh mục
                cmbcategory.SelectedIndex = -1;           // Mặc định không chọn
            }



        }

        private void btndeletecategory_Click(object sender, EventArgs e)
        {
            using (var context = new ProductDBContext())
            {
                // Kiểm tra xem có dòng nào được chọn không
                if (dataGridViewCategory.SelectedRows.Count > 0)
                {
                    int categoryId = Convert.ToInt32(dataGridViewCategory.SelectedRows[0].Cells["Id"].Value);

                    // Tìm Category theo ID
                    var category = context.Categories.Find(categoryId);

                    if (category != null)
                    {
                        // Xóa Category
                        context.Categories.Remove(category);
                        context.SaveChanges();
                        MessageBox.Show("Xóa thành công!");

                        // Tải lại danh sách
                        LoadCategoryData();
                    }
                    else
                    {
                        MessageBox.Show("Không tìm thấy danh mục!");
                    }
                }
                else
                {
                    MessageBox.Show("Vui lòng chọn một danh mục để xóa.");
                }
            }
        }

        private void btnfixcategory_Click(object sender, EventArgs e)
        {
            using (var context = new ProductDBContext())
            {
                // Kiểm tra xem có dòng nào được chọn không
                if (dataGridViewCategory.SelectedRows.Count > 0)
                {
                    int categoryId = Convert.ToInt32(dataGridViewCategory.SelectedRows[0].Cells["Id"].Value);

                    // Tìm Category theo ID
                    var category = context.Categories.Find(categoryId);

                    if (category != null)
                    {
                        // Cập nhật thông tin Category
                        category.CategoryName = txtcatname.Text;

                        context.SaveChanges();
                        MessageBox.Show("Cập nhật thành công!");

                        // Tải lại danh sách
                        LoadCategoryData();
                    }
                    else
                    {
                        MessageBox.Show("Không tìm thấy danh mục!");
                    }
                }
                else
                {
                    MessageBox.Show("Vui lòng chọn một danh mục để cập nhật.");
                }
            }
        }

        private void dataGridViewCategory_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridViewCategory.SelectedRows.Count > 0)
            {
                // Lấy thông tin từ dòng được chọn
                txtcatname.Text = dataGridViewCategory.SelectedRows[0].Cells["CategoryName"].Value.ToString();
            }
        }

        private void btnadduser_Click(object sender, EventArgs e)
        {
            using (var context = new ProductDBContext())
            {
                var user = new User
                {
                    username = txtusername.Text,
                    password = txtpassword.Text,
                    email = txtemail.Text // Sử dụng Email thay vì Role
                };

                context.Users.Add(user);
                context.SaveChanges();
                MessageBox.Show("Thêm người dùng thành công!");

                LoadUserData(); // Tải lại danh sách
            }
        }

        private void btnfixuser_Click(object sender, EventArgs e)
        {
            using (var context = new ProductDBContext())
            {
                if (dataGridViewUser.SelectedRows.Count > 0)
                {
                    int userId = Convert.ToInt32(dataGridViewUser.SelectedRows[0].Cells["Id"].Value);
                    var user = context.Users.Find(userId);

                    if (user != null)
                    {
                        user.username = txtusername.Text;
                        user.password = txtpassword.Text;
                        user.email = txtemail.Text; // Cập nhật Email

                        context.SaveChanges();
                        MessageBox.Show("Cập nhật người dùng thành công!");

                        LoadUserData(); // Tải lại danh sách
                    }
                    else
                    {
                        MessageBox.Show("Không tìm thấy người dùng!");
                    }
                }
                else
                {
                    MessageBox.Show("Vui lòng chọn một người dùng để cập nhật.");
                }
            }

        }
        private void LoadUserData()
        {
            using (var context = new ProductDBContext())
            {
                var users = context.Users
                    .Select(u => new
                    {
                        u.Id,
                        u.username,
                        u.email // Hiển thị Email thay vì Role
                    })
                    .ToList();

                dataGridViewUser.DataSource = users;
            }
        }

        private void btndeleteuser_Click(object sender, EventArgs e)
        {
            using (var context = new ProductDBContext())
            {
                if (dataGridViewUser.SelectedRows.Count > 0)
                {
                    int userId = Convert.ToInt32(dataGridViewUser.SelectedRows[0].Cells["Id"].Value);
                    var user = context.Users.Find(userId);

                    if (user != null)
                    {
                        context.Users.Remove(user);
                        context.SaveChanges();
                        MessageBox.Show("Xóa người dùng thành công!");

                        LoadUserData(); // Tải lại danh sách
                    }
                    else
                    {
                        MessageBox.Show("Không tìm thấy người dùng!");
                    }
                }
                else
                {
                    MessageBox.Show("Vui lòng chọn một người dùng để xóa.");
                }
            }
        }

        private void dataGridViewUser_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridViewUser.SelectedRows.Count > 0)
            {
                txtusername.Text = dataGridViewUser.SelectedRows[0].Cells["Username"].Value.ToString();
                txtemail.Text = dataGridViewUser.SelectedRows[0].Cells["Email"].Value.ToString(); // Hiển thị Email
            }
        }

        private void LoadInvoices()
        {
            using (var context = new ProductDBContext())
            {
                var invoices = context.Invoices.ToList();
                dataGridViewInvoices.DataSource = invoices;
            }
        }

        private void LoadInvoiceDetails(int invoiceId)
        {
            using (var context = new ProductDBContext())
            {
                var invoiceDetails = context.InvoiceDetails
                    .Where(d => d.InvoiceId == invoiceId)
                    .ToList();

                dataGridViewInvoiceDetails.DataSource = invoiceDetails;
            }
        }

        private void btnAddInvoice_Click(object sender, EventArgs e)
        {
            using (var context = new ProductDBContext())
            {
                var invoice = new Invoice
                {
                    CustomerName = txtcustomername.Text,
                    CustomerPhone = txtcustomerphone.Text,
                    CustomerAddress = txtcustomeraddress.Text,
                    InvoiceDate = DateTime.Now, // Tự động lấy ngày hiện tại
                    TotalAmount = decimal.Parse(txttotalamount.Text)
                };

                context.Invoices.Add(invoice);
                context.SaveChanges();

                // Hiển thị thông tin hóa đơn vừa thêm
                txtinvoiceid.Text = invoice.InvoiceId.ToString(); // Lấy InvoiceId sau khi lưu
                txtinvoicedate.Text = invoice.InvoiceDate.ToString("yyyy-MM-dd HH:mm:ss");

                MessageBox.Show("Hóa đơn đã được thêm thành công!");

                // Tải lại danh sách hóa đơn
                LoadInvoices();
            }
        }

        private void dataGridViewInvoices_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridViewInvoices.SelectedRows.Count > 0)
            {
                var selectedRow = dataGridViewInvoices.SelectedRows[0];

                // Lấy dữ liệu từ dòng được chọn
                txtinvoiceid.Text = selectedRow.Cells["InvoiceId"].Value.ToString();
                txtcustomername.Text = selectedRow.Cells["CustomerName"].Value.ToString();
                txtcustomerphone.Text = selectedRow.Cells["CustomerPhone"].Value.ToString();
                txtcustomeraddress.Text = selectedRow.Cells["CustomerAddress"].Value.ToString();
                txttotalamount.Text = selectedRow.Cells["TotalAmount"].Value.ToString();
                txtinvoicedate.Text = selectedRow.Cells["InvoiceDate"].Value.ToString();
            }
        }

        private void btnAddInvoiceDetails_Click(object sender, EventArgs e)
        {
            // Kiểm tra tên sản phẩm
            if (string.IsNullOrWhiteSpace(txtproductname.Text))
            {
                MessageBox.Show("Vui lòng nhập tên sản phẩm!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Kiểm tra và chuyển đổi giá trị số lượng
            if (!int.TryParse(txtquantity.Text, out int quantity))
            {
                MessageBox.Show("Vui lòng nhập số lượng hợp lệ!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Kiểm tra và chuyển đổi giá trị giá đơn vị
            if (!decimal.TryParse(txtunitprice.Text, out decimal unitPrice))
            {
                MessageBox.Show("Vui lòng nhập giá hợp lệ!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            // Kiểm tra dữ liệu nhập
            if (string.IsNullOrWhiteSpace(txtproductname.Text))
            {
                MessageBox.Show("Vui lòng nhập tên sản phẩm!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(txtquantity.Text) || !int.TryParse(txtquantity.Text, out int Quantity))
            {
                MessageBox.Show("Vui lòng nhập số lượng hợp lệ!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(txtunitprice.Text) || !decimal.TryParse(txtunitprice.Text, out decimal unitprice))
            {
                MessageBox.Show("Vui lòng nhập giá hợp lệ!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Tính toán thành tiền
            decimal subtotal = quantity * unitPrice;

            // Thêm sản phẩm vào cơ sở dữ liệu
            using (var context = new ProductDBContext())
            {
                var invoiceDetail = new InvoiceDetail
                {
                    InvoiceId = int.Parse(txtinvoiceid.Text), // Đảm bảo txtInvoiceId có giá trị
                    ProductName = txtproductname.Text,
                    Quantity = quantity,
                    UnitPrice = unitPrice,
                    Subtotal = subtotal
                };

                context.InvoiceDetails.Add(invoiceDetail);
                context.SaveChanges();
                MessageBox.Show("Thêm sản phẩm vào hóa đơn thành công!");
            }

            // Tải lại chi tiết hóa đơn
            LoadInvoiceDetails(int.Parse(txtinvoiceid.Text));
            ClearInvoiceDetailInputs();
        }

        private void btnDeleteInvoice_Click(object sender, EventArgs e)
        {
            using (var context = new ProductDBContext())
            {
                int invoiceId = int.Parse(txtinvoiceid.Text); // Lấy Invoice ID từ TextBox
                var invoice = context.Invoices.Find(invoiceId);

                if (invoice != null)
                {
                    context.Invoices.Remove(invoice);
                    context.SaveChanges();
                    MessageBox.Show("Hóa đơn đã được xóa!");

                    // Tải lại danh sách hóa đơn
                    LoadInvoices();
                }
            }
        }

        private void btnsavecategory_Click(object sender, EventArgs e)
        {
            using (var context = new ProductDBContext())
            {
                // Kiểm tra nếu ô ID không trống, nghĩa là đang update
                if (!string.IsNullOrWhiteSpace(txtcatid.Text))
                {
                    int categoryId = int.Parse(txtcatid.Text);

                    // Tìm danh mục trong database
                    var category = context.Categories.Find(categoryId);

                    if (category != null)
                    {
                        // Cập nhật thông tin danh mục
                        category.CategoryName = txtcatname.Text;

                        context.SaveChanges();
                        MessageBox.Show("Cập nhật danh mục thành công!");
                    }
                    else
                    {
                        MessageBox.Show("Không tìm thấy danh mục để cập nhật.");
                    }
                }
                else
                {
                    // Nếu ô ID trống, thêm mới danh mục
                    var category = new Category
                    {
                        CategoryName = txtcatname.Text
                    };

                    context.Categories.Add(category);
                    context.SaveChanges();
                    MessageBox.Show("Thêm danh mục mới thành công!");
                }

                // Tải lại danh sách danh mục sau khi thêm hoặc sửa
                LoadCategoryData();
                ClearCategoryInputs();
            }
        }
        private void ClearCategoryInputs()
        {
            txtcatid.Clear();
            txtcatname.Clear();
        }

        private void btnsaveuser_Click(object sender, EventArgs e)
        {
            using (var context = new ProductDBContext())
            {
                // Kiểm tra xem Username có tồn tại trong cơ sở dữ liệu hay không
                var user = context.Users.FirstOrDefault(u => u.username == txtusername.Text);

                if (user != null)
                {
                    // Nếu người dùng tồn tại, cập nhật thông tin
                    user.password = txtpassword.Text;
                    user.email = txtemail.Text;

                    context.SaveChanges();
                    MessageBox.Show("Cập nhật thông tin người dùng thành công!");
                }
                else
                {
                    // Nếu người dùng không tồn tại, thêm mới
                    var newUser = new User
                    {
                        username = txtusername.Text,
                        password = txtpassword.Text,
                        email = txtemail.Text
                    };

                    context.Users.Add(newUser);
                    context.SaveChanges();
                    MessageBox.Show("Thêm người dùng mới thành công!");
                }

                // Tải lại danh sách người dùng sau khi thêm hoặc sửa
                LoadUserData();
                ClearUserInputs();
            }

        }
        private void ClearUserInputs()
        {
            txtusername.Clear();
            txtpassword.Clear();
            txtemail.Clear();
        }

        private void ClearInvoiceDetailInputs()
        {
            txtproductname.Clear();
            txtquantity.Clear();
            txtunitprice.Clear();
        }

        private void btnDeleteInvoiceDetails_Click(object sender, EventArgs e)
        {
            // Kiểm tra xem người dùng có chọn một hàng trong DataGridView hay không
            if (dataGridViewInvoiceDetails.SelectedRows.Count == 0)
            {
                MessageBox.Show("Vui lòng chọn một mục cần xóa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Lấy Invoice Detail ID từ hàng được chọn
            var selectedRow = dataGridViewInvoiceDetails.SelectedRows[0];
            if (!int.TryParse(selectedRow.Cells["InvoiceDetailID"].Value?.ToString(), out int invoiceDetailId))
            {
                MessageBox.Show("Không thể lấy ID của hóa đơn chi tiết để xóa!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Xác nhận trước khi xóa
            var result = MessageBox.Show("Bạn có chắc chắn muốn xóa hóa đơn chi tiết này không?", "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.No)
            {
                return;
            }

            try
            {
                // Xóa hóa đơn chi tiết từ cơ sở dữ liệu
                using (var context = new ProductDBContext())
                {
                    var invoiceDetail = context.InvoiceDetails.FirstOrDefault(i => i.InvoiceDetailId == invoiceDetailId);
                    if (invoiceDetail != null)
                    {
                        context.InvoiceDetails.Remove(invoiceDetail);
                        context.SaveChanges();
                        MessageBox.Show("Xóa hóa đơn chi tiết thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        // Tải lại dữ liệu vào DataGridView sau khi xóa
                        LoadInvoiceDetails(int.Parse(txtinvoiceid.Text)); // Đảm bảo txtInvoiceId chứa ID hóa đơn hợp lệ
                    }
                    else
                    {
                        MessageBox.Show("Hóa đơn chi tiết không tồn tại trong cơ sở dữ liệu!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Đã xảy ra lỗi khi xóa hóa đơn chi tiết: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dataGridViewInvoiceDetails_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridViewInvoiceDetails.SelectedRows.Count > 0)
            {
                var selectedRow = dataGridViewInvoiceDetails.SelectedRows[0];

                // Lấy dữ liệu từ các cột của hàng được chọn và điền vào TextBox
                txtinvoicedetailid.Text = selectedRow.Cells["InvoiceDetailID"].Value?.ToString();
                txtinvoiceid.Text = selectedRow.Cells["InvoiceID"].Value?.ToString();
                txtproductname.Text = selectedRow.Cells["ProductName"].Value?.ToString();
                txtquantity.Text = selectedRow.Cells["Quantity"].Value?.ToString();
                txtunitprice.Text = selectedRow.Cells["UnitPrice"].Value?.ToString();
                txtsubtotal.Text = selectedRow.Cells["SubTotal"].Value?.ToString();
            }
        }

        private void btnUpdateInVoiceDetails_Click(object sender, EventArgs e)
        {
            if (dataGridViewInvoiceDetails.SelectedRows.Count == 0)
            {
                MessageBox.Show("Vui lòng chọn một mục cần sửa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Lấy ID của hóa đơn chi tiết từ hàng được chọn
            var selectedRow = dataGridViewInvoiceDetails.SelectedRows[0];
            if (!int.TryParse(selectedRow.Cells["InvoiceDetailID"].Value?.ToString(), out int invoiceDetailId))
            {
                MessageBox.Show("Không thể lấy ID của hóa đơn chi tiết để sửa!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                // Cập nhật thông tin trong cơ sở dữ liệu
                using (var context = new ProductDBContext())
                {
                    var invoiceDetail = context.InvoiceDetails.FirstOrDefault(i => i.InvoiceDetailId == invoiceDetailId);
                    if (invoiceDetail != null)
                    {
                        invoiceDetail.InvoiceDetailId = int.Parse(txtinvoiceid.Text);
                        invoiceDetail.ProductName = txtproductname.Text;
                        invoiceDetail.Quantity = int.Parse(txtquantity.Text);
                        invoiceDetail.UnitPrice = decimal.Parse(txtunitprice.Text);
                        invoiceDetail.Subtotal = decimal.Parse(txtsubtotal.Text);

                        context.SaveChanges();
                        MessageBox.Show("Cập nhật thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        // Tải lại dữ liệu
                        LoadInvoiceDetails(invoiceDetail.InvoiceDetailId);
                    }
                    else
                    {
                        MessageBox.Show("Hóa đơn chi tiết không tồn tại trong cơ sở dữ liệu!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Đã xảy ra lỗi khi sửa hóa đơn chi tiết: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnsaveinvoicedetails_Click(object sender, EventArgs e)
        {
            try
            {
                using (var context = new ProductDBContext())
                {
                    foreach (DataGridViewRow row in dataGridViewInvoiceDetails.Rows)
                    {
                        // Lấy dữ liệu từ DataGridView
                        if (row.Cells["InvoiceDetailID"].Value != null)
                        {
                            int invoiceDetailId = int.Parse(row.Cells["InvoiceDetailID"].Value.ToString());
                            var invoiceDetail = context.InvoiceDetails.FirstOrDefault(i => i.InvoiceDetailId == invoiceDetailId);

                            if (invoiceDetail != null)
                            {
                                // Cập nhật dữ liệu trong cơ sở dữ liệu
                                invoiceDetail.InvoiceDetailId = int.Parse(row.Cells["InvoiceID"].Value.ToString());
                                invoiceDetail.ProductName = row.Cells["ProductName"].Value.ToString();
                                invoiceDetail.Quantity = int.Parse(row.Cells["Quantity"].Value.ToString());
                                invoiceDetail.UnitPrice = decimal.Parse(row.Cells["UnitPrice"].Value.ToString());
                                invoiceDetail.Subtotal = decimal.Parse(row.Cells["SubTotal"].Value.ToString());
                            }
                        }
                        else
                        {
                            // Thêm mới nếu InvoiceDetailID không tồn tại
                            var newInvoiceDetail = new InvoiceDetail
                            {
                                InvoiceDetailId = int.Parse(row.Cells["InvoiceID"].Value.ToString()),
                                ProductName = row.Cells["ProductName"].Value.ToString(),
                                Quantity = int.Parse(row.Cells["Quantity"].Value.ToString()),
                                UnitPrice = decimal.Parse(row.Cells["UnitPrice"].Value.ToString()),
                                Subtotal = decimal.Parse(row.Cells["SubTotal"].Value.ToString())
                            };

                            context.InvoiceDetails.Add(newInvoiceDetail);
                        }
                    }

                    // Lưu thay đổi vào cơ sở dữ liệu
                    context.SaveChanges();
                    MessageBox.Show("Lưu dữ liệu thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Tải lại dữ liệu
                    LoadInvoiceDetails(int.Parse(txtinvoiceid.Text));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Đã xảy ra lỗi khi lưu dữ liệu: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnUpdateInvoice_Click(object sender, EventArgs e)
        {
            if (dataGridViewInvoices.SelectedRows.Count == 0)
            {
                MessageBox.Show("Vui lòng chọn một hóa đơn cần sửa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Lấy ID của hóa đơn từ hàng được chọn
            var selectedRow = dataGridViewInvoices.SelectedRows[0];
            if (!int.TryParse(selectedRow.Cells["InvoiceID"].Value?.ToString(), out int invoiceId))
            {
                MessageBox.Show("Không thể lấy ID của hóa đơn để sửa!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                // Cập nhật thông tin hóa đơn trong cơ sở dữ liệu
                using (var context = new ProductDBContext())
                {
                    var invoice = context.Invoices.FirstOrDefault(i => i.InvoiceId == invoiceId);
                    if (invoice != null)
                    {
                        invoice.InvoiceDate = DateTime.Parse(txtinvoicedate.Text);
                        invoice.CustomerName = txtcustomername.Text;
                        invoice.TotalAmount = decimal.Parse(txttotalamount.Text);

                        context.SaveChanges();
                        MessageBox.Show("Cập nhật hóa đơn thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        // Tải lại dữ liệu
                        LoadInvoices();
                    }
                    else
                    {
                        MessageBox.Show("Hóa đơn không tồn tại trong cơ sở dữ liệu!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Đã xảy ra lỗi khi sửa hóa đơn: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnsaveinvoice_Click(object sender, EventArgs e)
        {
            try
            {
                using (var context = new ProductDBContext())
                {
                    foreach (DataGridViewRow row in dataGridViewInvoices.Rows)
                    {
                        if (row.Cells["InvoiceID"].Value != null)
                        {
                            // Nếu đã tồn tại InvoiceID, cập nhật hóa đơn
                            int invoiceId = int.Parse(row.Cells["InvoiceID"].Value.ToString());
                            var invoice = context.Invoices.FirstOrDefault(i => i.InvoiceId == invoiceId);

                            if (invoice != null)
                            {
                                invoice.InvoiceDate = DateTime.Parse(row.Cells["InvoiceDate"].Value.ToString());
                                invoice.CustomerName = row.Cells["CustomerName"].Value.ToString();
                                invoice.TotalAmount = decimal.Parse(row.Cells["TotalAmount"].Value.ToString());
                            }
                        }
                        else
                        {
                            // Nếu InvoiceID không tồn tại, thêm mới
                            var newInvoice = new Invoice
                            {
                                InvoiceDate = DateTime.Parse(row.Cells["InvoiceDate"].Value.ToString()),
                                CustomerName = row.Cells["CustomerName"].Value.ToString(),
                                TotalAmount = decimal.Parse(row.Cells["TotalAmount"].Value.ToString())
                            };

                            context.Invoices.Add(newInvoice);
                        }
                    }

                    // Lưu thay đổi vào cơ sở dữ liệu
                    context.SaveChanges();
                    MessageBox.Show("Lưu dữ liệu hóa đơn thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Tải lại dữ liệu
                    LoadInvoices();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Đã xảy ra lỗi khi lưu dữ liệu hóa đơn: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count >= 0)
            {
                // Lấy thông tin từ dòng được chọn
                txtid.Text = dataGridView1.SelectedRows[0].Cells["Id"].Value.ToString();
                txtname.Text = dataGridView1.SelectedRows[0].Cells["Name"].Value.ToString();
                txtdescription.Text = dataGridView1.SelectedRows[0].Cells["Description"].Value.ToString();
                txtquantity.Text = dataGridView1.SelectedRows[0].Cells["Quantity"].Value.ToString();
                txtprice.Text = dataGridView1.SelectedRows[0].Cells["Price"].Value.ToString();
                cmbcategory.SelectedValue = dataGridView1.SelectedRows[0].Cells["CategoryId"].Value.ToString();


            }
        }

        private int GetCategoryIdByName(string categoryName, ProductDBContext context)
        {
            var category = context.Categories.FirstOrDefault(c => c.CategoryName == categoryName);
            if (category != null)
            {
                return category.Id; // Trả về CategoryID nếu tìm thấy
            }
            else
            {
                throw new Exception($"Không tìm thấy danh mục với tên: {categoryName}");
            }
        }

        private void btnsave_Click(object sender, EventArgs e)
        {

            try
            {
                using (var context = new ProductDBContext())
                {
                    foreach (DataGridViewRow row in dataGridView1.Rows)
                    {
                        // Kiểm tra dữ liệu ID sản phẩm
                        if (row.Cells["Id"].Value != null && int.TryParse(row.Cells["Id"].Value.ToString(), out int productId))
                        {
                            // Nếu ProductID tồn tại, cập nhật thông tin sản phẩm
                            var product = context.Products.FirstOrDefault(p => p.Id == productId);
                            if (product != null)
                            {
                                product.Name = row.Cells["Name"].Value.ToString();
                                product.Description = row.Cells["Description"].Value?.ToString();
                                product.Price = double.Parse(row.Cells["Price"].Value.ToString());
                                product.CategoryId = GetCategoryIdByName(row.Cells["CategoryName"].Value.ToString(), context);
                            }
                        }
                        else
                        {
                            // Nếu không có ProductID, thêm mới sản phẩm
                            var newProduct = new Product
                            {
                                Name = row.Cells["Name"].Value.ToString(),
                                Description = row.Cells["Description"].Value?.ToString(),
                                Price = double.Parse(row.Cells["Price"].Value.ToString()),
                                CategoryId = GetCategoryIdByName(row.Cells["CategoryName"].Value.ToString(), context)
                            };

                            context.Products.Add(newProduct);
                        }
                    }

                    // Lưu thay đổi vào cơ sở dữ liệu
                    context.SaveChanges();
                    MessageBox.Show("Lưu sản phẩm thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Tải lại dữ liệu sản phẩm
                    LoadData();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Đã xảy ra lỗi khi lưu sản phẩm: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void dataGridView1_SelectionChanged_1(object sender, EventArgs e)
        {


            if (dataGridView1.SelectedRows.Count > 0)
            {
                var selectedRow = dataGridView1.SelectedRows[0];

                // Gán dữ liệu từ dòng được chọn sang các ô nhập liệu
                txtid.Text = selectedRow.Cells["Id"].Value.ToString();
                txtname.Text = selectedRow.Cells["Name"].Value.ToString();
                txtdescription.Text = selectedRow.Cells["Description"].Value.ToString();
                txtquantity.Text = selectedRow.Cells["Quantity"].Value.ToString();
                txtprice.Text = selectedRow.Cells["Price"].Value.ToString();

                // Gán CategoryId vào ComboBox
                if (selectedRow.Cells["CategoryId"].Value != null)
                {
                    cmbcategory.SelectedValue = Convert.ToInt32(selectedRow.Cells["CategoryId"].Value);
                }
                else
                {
                    cmbcategory.SelectedIndex = -1; // Nếu không có giá trị
                }


            }

        }
    }

        
    
}
