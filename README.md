# AspNetCore.Weixin

## Settings
https://mp.weixin.qq.com

- 公众号开发信息:AppID,AppSecret,IP WhiteList

- 服务器配置:启用
ServerUrl: https://xxx/wx
ServerToken: xxx
EncodingAESKey: xxx
MessagingMode: PlainText/Encrypted/Any

## Basic Features
- AccessToken
每日限调用2000次，因此必须管理好。
1.内存：适用于本地测试（但不推荐）
2.redis：适用于正式上线平台（推荐）
3.本地文件：适用于测试环境
4.mongo：（未实现）

- MessageSignature
- MessageEncrypt/Decrypt

## Message 普通消息
- 接收普通消息
text,image,voice,video,shortvideo,location,link
(5s内必须响应）

## AdvancedMessage 客服消息
48h内响应，客户通过微信发送消息、点击菜单、提交关注、扫场景二维码、提交支付、提交维权后自动重置计时器

- 响应消息类型
typing（对方正在输入...）,text,image,voice,video,music,news外链图文（限1条）,mpnews内链图文（限1条）,msgmenu菜单（例如服务评价调查）,wxcard（卡券）,miniprogrampage（小程序卡片）

- 管理官服人员
增、删、改、头像
（暂未实现）

## Bulletin/BroadcastMessage 群发消息
服务号每月4条图文
（暂不实现，直接用微信后台功能）

## TemplateMessage 模版消息
限2个行业，25个消息模版
(每月可改1次，每日限调10万次，订阅人数大于10万另说)

## Recommend 一次性订阅消息
向客户转推订阅
（暂未实现）
