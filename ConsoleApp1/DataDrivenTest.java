package tests;

import java.io.File;
import java.io.FileInputStream;
import java.io.FileOutputStream;
import java.io.IOException;

import org.apache.poi.xssf.usermodel.XSSFSheet;
import org.apache.poi.xssf.usermodel.XSSFWorkbook;
import org.junit.After;
import org.junit.Before;
import org.junit.Test;

import Steps.LoginSteps;
import Steps.LogoutSteps;
import common.WebDriverFactory;

public class DataDrivenLoginTest extends BaseTest {

	// @Steps
	LoginSteps loginSteps = new LoginSteps();
	LogoutSteps logoutSteps = new LogoutSteps();

	@Test
	public void ReadData() throws IOException, Exception {

		XSSFWorkbook workbook;
		XSSFSheet sheet;
		// Import excel sheet.
		File src = new File("C:\\Users\\matthew.dotson1\\Documents\\logins.xlsx");
		// Load the file.
		FileInputStream fis = new FileInputStream(src);
		// Load he workbook.
		workbook = new XSSFWorkbook(fis);
		// Load the sheet in which data is stored.
		sheet = workbook.getSheet("Sheet1");

		for (int i = 0; i <= sheet.getLastRowNum(); i++) {
			/*
			 * I have added test data in the cell A2 as "testemailone@test.com"
			 * and B2 as "password" Cell A2 = row 1 and column 0. It reads first
			 * row as 0, second row as 1 and so on and first column (A) as 0 and
			 * second column (B) as 1 and so on
			 */

			String message = this.runTest(sheet.getRow(i).getCell(0).toString());

			// To write data in the excel
			FileOutputStream fos = new FileOutputStream(src);
			// Message to be written in the excel sheet
			// String message = "Pass";
			// Create cell where data needs to be written.
			sheet.getRow(i).createCell(2).setCellValue(message.toString());

			// finally write content
			workbook.write(fos);
			// close the file
			fos.close();

		}
		workbook.close();
	}

	public String runTest(String user) {

		try {

			this.loginSteps.login_as(this.driver, user);
			this.logoutSteps.logout(this.driver);
		} catch (Exception e) {
			assert (false);
		}
		return "Pass";
	}

	@Override
	@Before
	public void setUp() throws Exception {
		this.driver = WebDriverFactory.getIEWebDriver();
	}

	@Override
	@After
	public void tearDown() throws Exception {
		this.driver.close();
	}
}
