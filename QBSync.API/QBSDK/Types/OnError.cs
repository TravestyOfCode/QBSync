using System.ComponentModel.DataAnnotations;

namespace QBSync.API.QBSDK;

public enum OnError
{
    [Display(Name = "Stop on Error")]
    stopOnError,

    [Display(Name = "Continue on Error")]
    continueOnError
}
