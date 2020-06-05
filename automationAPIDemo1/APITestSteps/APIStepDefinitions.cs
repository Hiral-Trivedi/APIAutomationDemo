using automationAPIDemo1.Models;
using System;
using TechTalk.SpecFlow;
using automationAPIDemo1.Routes;
using System.Data;
using NUnit.Framework;

namespace automationAPIDemo1.APITestSteps
{
    [Binding]
    public class APIDemoSteps
    {
        FeatureFileData fromFeature;
        string URL;
        string response;
        public static DataRow[] dtrwauthparam;

        public APIDemoSteps()
        {
            fromFeature = new FeatureFileData();
        }
        [Given(@"I have location and appid as parameters")]
        public void GivenIHaveLocationAndAppidAsParameters()
        {
            URL = "https://samples.openweathermap.org/";
        }

        [When(@"I post this request to serrver")]
        public void WhenIPostThisRequestToSerrver()
        {
            fromFeature.q = "London,us";
            fromFeature.appid = "b6907d289e10d714a6e88b30761fae22";

            DataTable dt = new DataTable();
            dt.Columns.Add("endpoint");
            dt.Columns.Add("parameter");
            dt.Columns.Add("parametervalue");
            dt.Columns.Add("parametertype");

            DataRow tempRow1 = dt.NewRow();
            tempRow1["endpoint"] = "forecast"; tempRow1["parameter"] = "q"; tempRow1["parametervalue"] = "London,us"; tempRow1["parametertype"] = "NA";
            dt.Rows.Add(tempRow1);

            DataRow tempRow2 = dt.NewRow();
            tempRow2["endpoint"] = "forecast"; tempRow2["parameter"] = "appid"; tempRow2["parametervalue"] = "b6907d289e10d714a6e88b30761fae22"; tempRow2["parametertype"] = "NA";


            dt.Rows.Add(tempRow2);
            dtrwauthparam = dt.Select("endpoint" + " = '" + "forecast" + "'");



            response = APILayer.GetTransactionPreferenceRequestToMuleSoft(URL, "forecast", fromFeature, dtrwauthparam);




        }

        [Then(@"the response should have (.*) days of data")]
        public void ThenTheResponseShouldHaveDaysOfData(int p0)
        {
            WeatherForecast resp = APILayer.GetJsonobj<WeatherForecast>(response);

            if (!(resp.cnt / 24 == 4))
            {
                Assert.Fail();
            }
        }

        [Then(@"the response should have forecast in hourly interval")]
        public void ThenTheResponseShouldHaveForecastInHourlyInterval()
        {
            WeatherForecast resp = APILayer.GetJsonobj<WeatherForecast>(response);
            
            for(int i = 0; i < resp.list.Count; i++)
            {
                if (i != 0)
                {
                    DateTime prevDateTime = Convert.ToDateTime(resp.list[i - 1].dt_txt);
                    DateTime currentDateTime = Convert.ToDateTime(resp.list[i].dt_txt);
                    prevDateTime=prevDateTime.AddHours(1);
                    if (prevDateTime != currentDateTime)
                    {
                        Assert.Fail();
                    }
                }
            }




        }

        [Then(@"in the response temp value should be in between temp_min and temp_max")]
        public void ThenInTheResponseTempValueShouldBeInBetweenTemp_MinAndTemp_Max()
        {
            WeatherForecast resp = APILayer.GetJsonobj<WeatherForecast>(response);
            foreach (var item in resp.list)
            {
                if (!(item.main.temp <= item.main.temp_max && item.main.temp >= item.main.temp_min))
                {
                    Assert.Fail();
                }
            }
        }

        [Then(@"description should be light rain if the value for weather id is (.*)")]
        public void ThenDescriptionShouldBeLightRainIfTheValueForWeatherIdIs(int p0)
        {
            WeatherForecast resp = APILayer.GetJsonobj<WeatherForecast>(response);

            foreach (var item in resp.list)
            {
                if (item.weather[0].id.ToString() == "500")
                {
                    if (item.weather[0].description.ToString() != "light rain")
                    {
                        Assert.Fail();
                    }
                }
            }
        }

        [Then(@"description should be clear sky if the value for weather id is (.*)")]
        public void ThenDescriptionShouldBeClearSkyIfTheValueForWeatherIdIs(int p0)
        {
            WeatherForecast resp = APILayer.GetJsonobj<WeatherForecast>(response);
            foreach (var item in resp.list)
            {
                if (item.weather[0].id.ToString() == "800")
                {
                    if (item.weather[0].description.ToString() != "clear sky")
                    {
                        Assert.Fail();
                    }
                }
            }


        }

    }
}
