namespace BricksWarehouse.Services;

public class WebMapperService : IMapper<ProductTypeEditWebModel>, IMapper<ProductType>
{
    ProductTypeEditWebModel IMapper<ProductTypeEditWebModel>.Map(object source)
    {
        var productType = (ProductType)source;
        var model = new ProductTypeEditWebModel
        {
            Id = productType.Id,
            Name = productType.Name,
            FormatNumber = productType.FormatNumber,
            Order = productType.Order,
            Units = productType.Units,
            Volume = productType.Volume,
            Weight = productType.Weight,
            IsDelete = productType.IsDelete,
        };
        return model;
    }

    ProductType IMapper<ProductType>.Map(object source)
    {
        var model = (ProductTypeEditWebModel)source;
        var productType = new ProductType
        {
            Id = model.Id,
            Name = model.Name,
            FormatNumber = model.FormatNumber,
            Order = model.Order,
            Units = model.Units,
            Volume = model.Volume,
            Weight = model.Weight,
            IsDelete = model.IsDelete,
        };
        return productType;
    }
}

