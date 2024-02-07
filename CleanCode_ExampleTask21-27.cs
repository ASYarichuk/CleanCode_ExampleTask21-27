private void CheckButtonClick(object sender, EventArgs e)
{
    string rawData = this.passportTextbox.Text.Trim().Replace(" ", string.Empty);
    int maxLengthRawData = 10;

    if (this.passportTextbox.Text.Trim() == "")
    {
        int num1 = (int)MessageBox.Show("Введите серию и номер паспорта");
    }

    if (rawData.Length < maxLengthRawData)
    {
        this.textResult.Text = "Неверный формат серии или номера паспорта";
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

        int num2 = (int)MessageBox.Show("Файл db.sqlite не найден. Положите файл в папку вместе с exe.");
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
        this.textResult.Text = "Паспорт «" + this.passportTextbox.Text + "» в списке участников дистанционного голосования НЕ НАЙДЕН";
    }

    connection.Close();
}

private void GetAccessText()
{
    if (Convert.ToBoolean(dataTable.Rows[0].ItemArray[1]))
    {
        this.textResult.Text = "По паспорту «" + this.passportTextbox.Text + "» доступ к бюллетеню на дистанционном электронном голосовании ПРЕДОСТАВЛЕН";
    }
    else
    {
        this.textResult.Text = "По паспорту «" + this.passportTextbox.Text + "» доступ к бюллетеню на дистанционном электронном голосовании НЕ ПРЕДОСТАВЛЯЛСЯ";
    }
}