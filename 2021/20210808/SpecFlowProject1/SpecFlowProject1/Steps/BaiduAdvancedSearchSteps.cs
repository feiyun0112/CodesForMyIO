using BoDi;
using Microsoft.Playwright;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading.Tasks;
using TechTalk.SpecFlow;

namespace SpecFlowProject1.Steps
{
    [Binding]
    public class BaiduAdvancedSearchSteps
    {
        [BeforeFeature]
        public static async Task BeforeFeature(IObjectContainer container)
        {
            var playwright = await Playwright.CreateAsync();
            //var browser = await playwright.Chromium.LaunchAsync(new() { Headless = true });
            var browser = await playwright.Chromium.LaunchAsync(new() { Headless = false, SlowMo=1000 });
            var page = await browser.NewPageAsync();

            container.RegisterInstanceAs<IPage>(page);
        }

        private IPage _page;
        public BaiduAdvancedSearchSteps(IPage page)
        {
            this._page = page;
        }

        [Given(@"打开百度首页")]
        public async Task Given打开百度首页()
        {
            await _page.GotoAsync("https://www.baidu.com/");
        }

        [When(@"鼠标悬停在“设置”按钮")]
        public async Task When鼠标悬停在设置按钮()
        {
            await _page.WaitForSelectorAsync("#s-usersetting-top");
            await _page.HoverAsync("#s-usersetting-top");
        }

        [When(@"点击设置菜单上的“高级搜索“按钮")]
        public async Task When点击设置菜单上的高级搜索按钮()
        {
            await _page.ClickAsync("a[href='//www.baidu.com/gaoji/advanced.html']");
        }

        [Then(@"弹出高级搜索页面")]
        public async Task Then弹出高级搜索页面()
        {
            var handle = await _page.WaitForSelectorAsync(".bdlayer.s-isindex-wrap.new-pmd.pfpanel");
            var style = await handle.GetAttributeAsync("style");
            Assert.IsTrue(style.Contains("display: block;"));
        }

        [Given(@"输入关键词""(.*)""")]
        public async Task Given输入关键词(string p0)
        {
            await _page.TypeAsync("input[name='q1']", p0);
        }

        [When(@"点击高级搜索页面上的“高级搜索""按钮")]
        public async Task When点击高级搜索页面上的高级搜索按钮()
        {
            _page = await _page.RunAndWaitForPopupAsync(async () =>
            { 
                await _page.ClickAsync(".advanced-search-btn");
            });
        }

        [Then(@"搜索框显示关键词""(.*)""")]
        public async Task Then搜索框显示关键词(string p0)
        {
            var handle = await _page.WaitForSelectorAsync("#kw");
            var text = await handle.GetAttributeAsync("value");
            Assert.AreEqual(p0, text);
        }
    }
}
