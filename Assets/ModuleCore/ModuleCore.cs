using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 核心模块，实现业务逻辑
/// </summary>
public class ModuleCore : Module<ModuleCore> {

    

}
/// <summary>
/// 模块基类
/// </summary>
/// <typeparam name="ModuleCore"></typeparam>
public class Module<ModuleCore> where ModuleCore : Module<ModuleCore>, new() {
    /// <summary> 模块单例 </summary>
    public static ModuleCore I => Instantiate();

    private static ModuleCore core;
    private static ModuleCore Instantiate() => core == null ? core = new ModuleCore() : core;
}