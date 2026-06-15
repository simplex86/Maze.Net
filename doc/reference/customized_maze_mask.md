# 自定义形状蒙版

``` csharp
public class CustomizedMazeMask
{
    public CustomizedMazeMask(bool[][] data);
}
```

## 参数

- **data** 图像数据

> [!NOTE]
> 像素灰度值大于128的像素点被看作生成迷宫的有效区域，否则为无效区域。