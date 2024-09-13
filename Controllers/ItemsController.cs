using Google.Apis.Sheets.v4.Data;
using Google.Apis.Sheets.v4;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static Google.Apis.Sheets.v4.SpreadsheetsResource.ValuesResource;
using WebApplication1.Model;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ItemsController : ControllerBase
    {
        const string SPREADSHEET_ID = "1Ma1VJD3z8Q_9z4w-K9YOXJAXj3onx3ak16xoLBv4eQ4";
        const string SHEET_NAME = "Sheet1";

        SpreadsheetsResource.ValuesResource _googleSheetValues;

        public ItemsController(GoogleSheetsHelper googleSheetsHelper)
        {
            _googleSheetValues = googleSheetsHelper.Service.Spreadsheets.Values;
        }

        [HttpGet]
        public IActionResult Get()
        {
            var range = $"{SHEET_NAME}!A:K";
            try
            {
                var request = _googleSheetValues.Get(SPREADSHEET_ID, range);
                var response = request.Execute();
                var values = response.Values;
                return Ok(ItemsMapper.MapFromRangeData(values));

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return BadRequest();
        }

        [HttpGet("{rowId}")]
        public IActionResult Get(int rowId)
        {
            var range = $"{SHEET_NAME}!A{rowId}:K{rowId}";
            var request = _googleSheetValues.Get(SPREADSHEET_ID, range);
            var response = request.Execute();
            var values = response.Values;

            return Ok(ItemsMapper.MapFromRangeData(values).FirstOrDefault());
        }

        [HttpPost]
        public IActionResult Post(PersonalInformation item)
        {
            var range = $"{SHEET_NAME}!A:K";
            var valueRange = new ValueRange
            {
                Values = ItemsMapper.MapToRangeData(item)
            };

            var appendRequest = _googleSheetValues.Append(valueRange, SPREADSHEET_ID, range);
            appendRequest.ValueInputOption = AppendRequest.ValueInputOptionEnum.USERENTERED;
            appendRequest.Execute();

            return CreatedAtAction(nameof(Get), item);
        }

        [HttpPut("{rowId}")]
        public IActionResult Put(int rowId, PersonalInformation item)
        {
            var range = $"{SHEET_NAME}!A{rowId}:K{rowId}";
            var valueRange = new ValueRange
            {
                Values = ItemsMapper.MapToRangeData(item)
            };

            var updateRequest = _googleSheetValues.Update(valueRange, SPREADSHEET_ID, range);
            updateRequest.ValueInputOption = UpdateRequest.ValueInputOptionEnum.USERENTERED;
            updateRequest.Execute();

            return NoContent();
        }

        [HttpDelete("{rowId}")]
        public IActionResult Delete(int rowId)
        {
            var range = $"{SHEET_NAME}!A{rowId}:K{rowId}";
            var requestBody = new ClearValuesRequest();

            var deleteRequest = _googleSheetValues.Clear(requestBody, SPREADSHEET_ID, range);
            deleteRequest.Execute();

            return NoContent();
        }
    }
}
