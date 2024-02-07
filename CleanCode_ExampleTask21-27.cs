private void CheckButtonClick(object sender, EventArgs e)
{
    string rawData = this.passportTextbox.Text.Trim().Replace(" ", string.Empty);
    int maxLengthRawData = 10;

    if (this.passportTextbox.Text.Trim() == "")
    {
        int num1 = (int)MessageBox.Show("������� ����� � ����� ��������");
    }

    if (rawData.Length < maxLengthRawData)
    {
        this.textResult.Text = "�������� ������ ����� ��� ������ ��������";
    }

    try
    {
        SendRequest();
    }
    catch (SQLiteException ex)
    {
        if (ex.ErrorCode != 1)
        {
            return;
        }

        int num2 = (int)MessageBox.Show("���� db.sqlite �� ������. �������� ���� � ����� ������ � exe.");
    }
}

private void SendRequest()
{
    string commandText = string.Format("select * from passports where num='{0}' limit 1;", (object)Form1.ComputeSha256Hash(rawData));
    string connectionString = string.Format("Data Source=" + Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\db.sqlite");

    SQLiteConnection connection = new SQLiteConnection(connectionString);

    connection.Open();

    SQLiteDataAdapter sqLiteDataAdapter = new SQLiteDataAdapter(new SQLiteCommand(commandText, connection));

    DataTable dataTable = new DataTable();

    sqLiteDataAdapter.Fill(dataTable);

    if (dataTable.Rows.Count > 0)
    {
        GetAccessText();
    }
    else
    {
        this.textResult.Text = "������� �" + this.passportTextbox.Text + "� � ������ ���������� �������������� ����������� �� ������";
    }

    connection.Close();
}

private void GetAccessText()
{
    if (Convert.ToBoolean(dataTable.Rows[0].ItemArray[1]))
    {
        this.textResult.Text = "�� �������� �" + this.passportTextbox.Text + "� ������ � ��������� �� ������������� ����������� ����������� ������������";
    }
    else
    {
        this.textResult.Text = "�� �������� �" + this.passportTextbox.Text + "� ������ � ��������� �� ������������� ����������� ����������� �� ��������������";
    }
}