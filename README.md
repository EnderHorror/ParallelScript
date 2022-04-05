# Parallel Script使用指南

![Image](https://github.com/EnderHorror/ParallelScript/blob/main/demo.gif)

```R
<anim:Rainbow><w=1>Wow!<w=1></anim>I <color=ff0000>love</color>card games!
  标记为彩虹特效      等待1s                      标记颜色
```

## 一.前言
1. 为什么要使用剧本语言:因为游戏的很多行为和事件都是跟随剧本执行的,如果不使用剧本语言,会让写程序的人为许多琐碎的功能而操劳,用这个可以一定程度上分担一部分程序的工作
什么是剧本语言:剧本语言就是编剧可以直接在剧本里就可以直接控制游戏的行为,比如移动镜头,控制角色的表情,控制音效的播放等等
 
2. 原理是什么: 基于Lua实现脚本执行,基于TextMeshPro实现富文本文字特效
 
 
# 二.基本语法
1. 语句
Parallel Script采用的是文本+<指令>的混合形式
比如:
```R
  不好!<lua:shake(1)>
  我们被袭击了
```
注意:<>块不支持嵌套

1. 函数
所有的函数需要放到<lua:>块中
调用方式 <lua:函数名(参数)>
 
4. 变量
Parallel语言中一切都可以是“变量”，但是Parallel不提供保存某种数据或数据结构的类型。如果你需要存储一些变量或调用一些原生方法，您可以在Parallel的<lua:>块内无缝地调用Lua接口。
 

# 三.支持的html标签

[官方文档](http://digitalnativestudios.com/textmeshpro/docs/rich-text/)

|Tags	| Summary|
|----|----|
|align |	文字对齐方式
|alpha, color |	颜色和透明度
|b, i |	粗体和斜体
|cspace |	文字行间距
|font |	字体
|indent	| 缩进
|line-height |	行高
|line-indent |	行缩进
|link |	文本元数据
|lowercase, uppercase, smallcaps |	字母大小写化
|margin |	盒子模型Margin
|mark|	标记文字
|mspace|	单个字符间距
|noparse|	不转义
|nobr|	文字不会被Warpping
|page|	Page break.
|pos|	水平位置
|size|	字体大小
|space|	空格长度
|sprite|	插入图片
|s, u	| 划掉线和下划线
|style|	自定义styles.
|sub, sup	|S上标和下标
|voffset	| 高度偏移
|width	|区域文字宽度


目前支持的文字特效:
```xml
<anim:Wobble></anim> //文字摇摆
<anim:Rainbow></anim>  //文字颜色彩虹
```

# 四.添加自己的Lua函数
利用LuaCenter里的Register函数可以注册

# 五.自定义文字特效
只需要创建一个新类然后实现ITextEffect接口
类名就是动画名称
如果改动画有参数需要调节 需要手动读取

# 六.Demo
请查看Demo.cs和Demo场景