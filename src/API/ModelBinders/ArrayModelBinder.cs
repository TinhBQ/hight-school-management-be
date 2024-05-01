using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel;
using System.Reflection;

namespace API.ModelBinders
{
    public class ArrayModelBinder : IModelBinder
    {
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            if (!bindingContext.ModelMetadata.IsEnumerableType)
            {
                bindingContext.Result = ModelBindingResult.Failed();
                return Task.CompletedTask;
            }

            var providedValue = bindingContext.ValueProvider
                .GetValue(bindingContext.ModelName)
                .ToString();

            if (string.IsNullOrEmpty(providedValue))
            {
                bindingContext.Result = ModelBindingResult.Success(null);
                return Task.CompletedTask;
            }

            var genericType = bindingContext.ModelType.GetTypeInfo().GenericTypeArguments[0];
            var converter = TypeDescriptor.GetConverter(genericType);

            var objectArray = providedValue.Split([","], StringSplitOptions.RemoveEmptyEntries)
                .Select(x =>
                {
                    try
                    {
                        return converter.ConvertFromString(x.Trim());
                    }
                    catch (FormatException)
                    {
                        // Chuỗi không đúng định dạng Guid, trả về giá trị mặc định hoặc xử lý khác tùy thuộc vào yêu cầu
                        return null;
                    }
                })
                .Where(x => x != null) // Lọc bỏ các giá trị null (chỉ có trong trường hợp chuỗi không đúng định dạng Guid)
                .ToArray();

            var guidArray = Array.CreateInstance(genericType, objectArray.Length);
            Array.Copy(objectArray, guidArray, objectArray.Length);

            bindingContext.Model = guidArray;
            bindingContext.Result = ModelBindingResult.Success(bindingContext.Model);
            return Task.CompletedTask;
        }
    }
}
