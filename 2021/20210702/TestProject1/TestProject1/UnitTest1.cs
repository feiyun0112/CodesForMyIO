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

            // 鼠标悬停在设置按钮，弹出菜单
            await page.WaitForSelectorAsync("#s-usersetting-top");
            await page.HoverAsync("#s-usersetting-top");

            // 点击高级搜索，弹出高级搜索窗口
            await page.ClickAsync("a[href='//www.baidu.com/gaoji/advanced.html']");

            // 输入搜索关键字
            await page.TypeAsync("input[name='q1']", "\"My IO\"");

            var page1 = await page.RunAndWaitForPopupAsync(async () =>
            {
                // 点击搜索
                await page.ClickAsync(".advanced-search-btn");
            });

            //检查文本框内容
            var handle = await page1.WaitForSelectorAsync("#kw");
            var text = await handle.GetAttributeAsync("value");
            Assert.AreEqual("\"My IO\"", text);
        }
    }
}