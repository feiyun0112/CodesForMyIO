using Microsoft.Playwright;
using NUnit.Framework;
using System.Threading.Tasks;

namespace TestProject1
{
    public class Tests
    {
        [Test]
        public async Task LoginInBaidu()
        {
            using var playwright = await Playwright.CreateAsync();
            await using var browser = await playwright.Chromium.LaunchAsync(new() { Headless = true });

            var page = await browser.NewPageAsync();
            await page.GotoAsync("https://www.baidu.com/");

            // �����ͣ�����ð�ť�������˵�
            await page.WaitForSelectorAsync("#s-usersetting-top");
            await page.HoverAsync("#s-usersetting-top");

            // ����߼������������߼���������
            await page.ClickAsync("a[href='//www.baidu.com/gaoji/advanced.html']");

            // ���������ؼ���
            await page.TypeAsync("input[name='q1']", "\"My IO\"");

            var page1 = await page.RunAndWaitForPopupAsync(async () =>
            {
                // �������
                await page.ClickAsync(".advanced-search-btn");
            });

            //����ı�������
            var handle = await page1.WaitForSelectorAsync("#kw");
            var text = await handle.GetAttributeAsync("value");
            Assert.AreEqual("\"My IO\"", text);
        }
    }
}