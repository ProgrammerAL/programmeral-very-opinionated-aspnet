using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Extensions.Options;

namespace FeedbackApp.Config;

public class StorageConfig
{
    [Required(AllowEmptyStrings = false), NotNull]
    public required string ConnectionString { get; set; }

    [Required(AllowEmptyStrings = false), NotNull]
    public required string TableName { get; set; }
}

[OptionsValidator]
public partial class StorageConfigValidateOptions : IValidateOptions<StorageConfig>
{
}
