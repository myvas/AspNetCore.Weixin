﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;

using System.Threading.Tasks;

namespace Myvas.AspNetCore.Weixin
{
    public class MenuApi : WeixinApiClient
    {
        public MenuApi(HttpClient client) : base(client)
        {
        }

        ///// <summary>
        ///// 特殊符号转义
        ///// </summary>
        ///// <param name="name"></param>
        ///// <returns></returns>
        //private static string ButtonNameEncode(string name)
        //{
        //    //直接用UrlEncode不行，显示内容超长
        //    return name.Replace("&", "%26");
        //}

        /// <summary>
        /// 创建菜单
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="buttonData">菜单内容</param>
        /// <returns></returns>
        public async Task<WeixinErrorJson> CreateMenu(string accessToken, ButtonGroup buttonData)
        {
            var api = "https://api.weixin.qq.com/cgi-bin/menu/create?access_token={0}";
            api = string.Format(api, accessToken);


            ////对特殊符号进行URL转义
            //foreach (var button in buttonData.button)
            //{
            //    button.name = ButtonNameEncode(button.name);//button.name.UrlEncode();
            //    if (button is SubButton)
            //    {
            //        var subButtonList = button as SubButton;
            //        foreach (var subButton in subButtonList.sub_button)
            //        {
            //            subButton.name = ButtonNameEncode(button.name);//button.name.UrlEncode();
            //        }
            //    }
            //}

            var tokenRequestParameters = new Dictionary<string, string>()
            {
                //{ "client_id", Options.ClientId },
                //{ "redirect_uri", redirectUri },
                //{ "client_secret", Options.ClientSecret },
                //{ "code", code },
                //{ "grant_type", "authorization_code" },
            };
            var requestContent = new FormUrlEncodedContent(tokenRequestParameters);

            return await PostContentAsJsonAsync<WeixinErrorJson>(api, requestContent);
        }

        /// <summary>
        /// 创建菜单
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="buttonData">菜单内容</param>
        /// <returns></returns>
        public async Task<WeixinErrorJson> CreateMenuAsync(string accessToken, string menuJson)
        {
            var api = "https://api.weixin.qq.com/cgi-bin/menu/create?access_token={0}";
            api = string.Format(api, accessToken);

            var requestContent = new StringContent(menuJson);
            return await PostContentAsJsonAsync<WeixinErrorJson>(api, requestContent);
        }


        #region GetMenu
        /// <summary>
        /// 获取单击按钮
        /// </summary>
        /// <param name="objs"></param>
        /// <returns></returns>
        [Obsolete("配合GetMenuFromJson方法使用")]
        private ClickMenuButton GetSingleButtonFromJsonObject(Dictionary<string, object> objs)
        {
            var sb = new ClickMenuButton()
            {
                key = objs["key"] as string,
                name = objs["name"] as string,
                type = objs["UploadMediaType"] as string
            };
            return sb;
        }


        /// <summary>
        /// 从JSON字符串获取菜单对象
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [Obsolete("此方法通过判断GetMenuResult并结合object类型转换得到结果。结果准确。但更推荐使用GetMenuFromJsonResult方法。")]
        public GetMenuResult GetMenuFromJson(string json)
        {
            var finalResult = new GetMenuResult();

            try
            {
                //@"{""menu"":{""button"":[{""UploadMediaType"":""click"",""name"":""单击测试"",""key"":""OneClick"",""sub_button"":[]},{""name"":""二级菜单"",""sub_button"":[{""UploadMediaType"":""click"",""name"":""返回文本"",""key"":""SubClickRoot_Text"",""sub_button"":[]},{""UploadMediaType"":""click"",""name"":""返回图文"",""key"":""SubClickRoot_News"",""sub_button"":[]},{""UploadMediaType"":""click"",""name"":""返回音乐"",""key"":""SubClickRoot_Music"",""sub_button"":[]}]}]}}"
                object jsonResult = null;

                jsonResult = JsonSerializer.Deserialize<object>(json);

                var fullResult = jsonResult as Dictionary<string, object>;
                if (fullResult != null && fullResult.ContainsKey("menu"))
                {
                    //得到菜单
                    var menu = fullResult["menu"];
                    var buttons = (menu as Dictionary<string, object>)["button"] as object[];

                    foreach (var rootButton in buttons)
                    {
                        var fullButton = rootButton as Dictionary<string, object>;
                        if (fullButton.ContainsKey("key") && !string.IsNullOrEmpty(fullButton["key"] as string))
                        {
                            //按钮，无下级菜单
                            finalResult.menu.button.Add(GetSingleButtonFromJsonObject(fullButton));
                        }
                        else
                        {
                            //二级菜单
                            var subButton = new SubMenuButton(fullButton["name"] as string);
                            finalResult.menu.button.Add(subButton);
                            foreach (var sb in fullButton["sub_button"] as object[])
                            {
                                subButton.sub_button.Add(GetSingleButtonFromJsonObject(sb as Dictionary<string, object>));
                            }
                        }
                    }
                }
                else if (fullResult != null && fullResult.ContainsKey("errmsg"))
                {
                    //菜单不存在
                    throw new WeixinException(fullResult["errmsg"] as string, null, null);
                }
            }
            catch (WeixinException)
            {
                finalResult = null;

                //如果没有惨淡会返回错误代码：46003：menu no exist
            }
            catch (Exception)
            {
                //其他异常
                finalResult = null;
            }
            return finalResult;
        }


        /// <summary>
        /// 获取当前菜单，如果菜单不存在，将返回null
        /// </summary>
        /// <param name="accessToken"></param>
        /// <returns></returns>
        public async Task<JsonDocument> GetMenuAsync(string accessToken)
        {
            var url = string.Format("https://api.weixin.qq.com/cgi-bin/menu/get?access_token={0}", accessToken);

            var responseMessage = await new HttpClient().GetAsync(url);
            responseMessage.EnsureSuccessStatusCode();
            var s = await responseMessage.Content.ReadAsStringAsync();
            var json = JsonDocument.Parse(s);
            return json;
        }

        public static GetMenuResult Parse(string json)
        {
            GetMenuResult finalResult;
            try
            {
                var jsonResult = JsonSerializer.Deserialize<GetMenuResultFull>(json);
                if (jsonResult.menu == null || jsonResult.menu.button.Count == 0)
                {
                    throw new WeixinException(jsonResult.ErrorMessage);
                }

                finalResult = GetMenuFromJsonResult(jsonResult);
            }
            catch (WeixinException)
            {
                finalResult = null;
            }

            return finalResult;
        }

        /// <summary>
        /// 根据微信返回的Json数据得到可用的GetMenuResult结果
        /// </summary>
        /// <param name="resultFull"></param>
        /// <returns></returns>
        public static GetMenuResult GetMenuFromJsonResult(GetMenuResultFull resultFull)
        {
            GetMenuResult result = null;
            try
            {
                //重新整理按钮信息
                ButtonGroup bg = new ButtonGroup();
                foreach (var rootButton in resultFull.menu.button)
                {
                    if (rootButton.name == null)
                    {
                        continue;//没有设置一级菜单
                    }
                    var availableSubButton = rootButton.sub_button.Count(z => !string.IsNullOrEmpty(z.name));//可用二级菜单按钮数量
                    if (availableSubButton == 0)
                    {
                        //底部单击按钮
                        if (rootButton.type == null ||
                            (rootButton.type.Equals("CLICK", StringComparison.OrdinalIgnoreCase)
                            && string.IsNullOrEmpty(rootButton.key)))
                        {
                            throw new WeixinException("单击按钮的key不能为空！");
                        }

                        if (rootButton.type.Equals("CLICK", StringComparison.OrdinalIgnoreCase))
                        {
                            //点击
                            bg.button.Add(new ClickMenuButton()
                            {
                                name = rootButton.name,
                                key = rootButton.key,
                                type = rootButton.type
                            });
                        }
                        else
                        {
                            //URL
                            bg.button.Add(new ViewMenuButton()
                            {
                                name = rootButton.name,
                                url = rootButton.url,
                                type = rootButton.type
                            });
                        }
                    }
                    else if (availableSubButton < 2)
                    {
                        throw new WeixinException("子菜单至少需要填写2个！");
                    }
                    else
                    {
                        //底部二级菜单
                        var subButton = new SubMenuButton(rootButton.name);
                        bg.button.Add(subButton);

                        foreach (var subSubButton in rootButton.sub_button)
                        {
                            if (subSubButton.name == null)
                            {
                                continue; //没有设置菜单
                            }

                            if (subSubButton.type.Equals("CLICK", StringComparison.OrdinalIgnoreCase)
                                && string.IsNullOrEmpty(subSubButton.key))
                            {
                                throw new WeixinException("单击按钮的key不能为空！");
                            }


                            if (subSubButton.type.Equals("CLICK", StringComparison.OrdinalIgnoreCase))
                            {
                                //点击
                                subButton.sub_button.Add(new ClickMenuButton()
                                {
                                    name = subSubButton.name,
                                    key = subSubButton.key,
                                    type = subSubButton.type
                                });
                            }
                            else
                            {
                                //URL
                                subButton.sub_button.Add(new ViewMenuButton()
                                {
                                    name = subSubButton.name,
                                    url = subSubButton.url,
                                    type = subSubButton.type
                                });
                            }
                        }
                    }
                }

                if (bg.button.Count < 2)
                {
                    throw new WeixinException("一级菜单按钮至少为2个！");
                }

                result = new GetMenuResult()
                {
                    menu = bg
                };
            }
            catch (Exception ex)
            {
                throw new WeixinException(ex.Message, ex);
            }
            return result;
        }

        #endregion

        /// <summary>
        /// 删除菜单
        /// </summary>
        /// <param name="accessToken"></param>
        /// <returns></returns>
        public async Task<WeixinErrorJson> DeleteMenu(string accessToken)
        {
            var url = string.Format("https://api.weixin.qq.com/cgi-bin/menu/delete?access_token={0}", accessToken);
            return await GetFromJsonAsync<WeixinErrorJson>(url);
        }
    }
}
