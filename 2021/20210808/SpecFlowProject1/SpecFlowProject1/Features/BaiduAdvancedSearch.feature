Feature: 百度高级搜索
    所有场景必须顺序执行
 
Scenario: （1）显示高级搜索页面
    Given 打开百度首页
    When 鼠标悬停在“设置”按钮
    And 点击设置菜单上的“高级搜索“按钮
    Then 弹出高级搜索页面
 
Scenario: （2）执行高级搜索
    Given 输入关键词"My IO"
    When 点击高级搜索页面上的“高级搜索"按钮
    Then 搜索框显示关键词"My IO"