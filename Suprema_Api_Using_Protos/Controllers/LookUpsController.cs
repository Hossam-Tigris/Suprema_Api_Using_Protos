
using Gsdk.Auth;
using Gsdk.Card;
using Gsdk.Device;
using Gsdk.Display;
using Gsdk.Face;
using Gsdk.Finger;
using Gsdk.Schedule;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Extensions;
using Microsoft.VisualBasic;
using Suprema_Api_Using_Protos.Helper;

namespace Suprema_Api_Using_Protos.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LookUpsController : ControllerBase
    {

        [HttpGet("languages")]
        public IActionResult GetLanguages() => Ok(LookUpsExtention.ToDtoList<LanguageType>());

        [HttpGet("background-types")]
        public IActionResult GetBackgroundTypes() => Ok(LookUpsExtention.ToDtoList<BackgroundType>());

        [HttpGet("background-themes")]
        public IActionResult GetBackgroundThemes() => Ok(LookUpsExtention.ToDtoList<BackgroundTheme>());

        [HttpGet("date-formats")]
        public IActionResult GetDateFormats() => Ok(LookUpsExtention.ToDtoList<Gsdk.Display.DateFormat>());

        [HttpGet("time-formats")]
        public IActionResult GetTimeFormats() => Ok(LookUpsExtention.ToDtoList<TimeFormat>());

        [HttpGet("sound-indices")]
        public IActionResult GetSoundIndices() => Ok(LookUpsExtention.ToDtoList<SoundIndex>());

        [HttpGet("auth-modes")]
        public IActionResult GetAuthModes() => Ok(LookUpsExtention.ToDtoList<AuthMode>());

        [HttpGet("operator-levels")]
        public IActionResult GetOperatorLevels() => Ok(LookUpsExtention.ToDtoList<OperatorLevel>());

        [HttpGet("face-detection-levels")]
        public IActionResult GetFaceDetectionLevels() => Ok(LookUpsExtention.ToDtoList<FaceDetectionLevel>());

        [HttpGet("global-apb-actions")]
        public IActionResult GetGlobalAPBActions() => Ok(LookUpsExtention.ToDtoList<GlobalAPBFailActionType>());

        [HttpGet("card-types")]
        public IActionResult GetCardTypes() => Ok(LookUpsExtention.ToDtoList<Gsdk.Card.Type>());

        [HttpGet("switch-types")]
        public IActionResult GetSwitchTypes() => Ok(LookUpsExtention.ToDtoList<SwitchType>());

        [HttpGet("led-colors")]
        public IActionResult GetLEDColors() => Ok(LookUpsExtention.ToDtoList<LEDColor>());

        [HttpGet("buzzer-tones")]
        public IActionResult GetBuzzerTones() => Ok(LookUpsExtention.ToDtoList<BuzzerTone>());

        [HttpGet("face-security-levels")]
        public IActionResult GetFaceSecurityLevels() => Ok(LookUpsExtention.ToDtoList<FaceSecurityLevel>());

        [HttpGet("face-enroll-thresholds")]
        public IActionResult GetFaceEnrollThresholds() => Ok(LookUpsExtention.ToDtoList<FaceEnrollThreshold>());

        [HttpGet("face-light-conditions")]
        public IActionResult GetFaceLightConditions() => Ok(LookUpsExtention.ToDtoList<FaceLightCondition>());

        [HttpGet("face-detect-sensitivities")]
        public IActionResult GetFaceDetectSensitivities() => Ok(LookUpsExtention.ToDtoList<FaceDetectSensitivity>());

        [HttpGet("face-lfd-levels")]
        public IActionResult GetFaceLFDLevels() => Ok(LookUpsExtention.ToDtoList<FaceLFDLevel>());

        [HttpGet("face-preview-options")]
        public IActionResult GetFacePreviewOptions() => Ok(LookUpsExtention.ToDtoList<FacePreviewOption>());

        [HttpGet("template-formats")]
        public IActionResult GetTemplateFormats() => Ok(LookUpsExtention.ToDtoList<TemplateFormat>());

        [HttpGet("holiday-recurrences")]
        public IActionResult GetHolidayRecurrences() => Ok(LookUpsExtention.ToDtoList<HolidayRecurrence>());
    }
}

