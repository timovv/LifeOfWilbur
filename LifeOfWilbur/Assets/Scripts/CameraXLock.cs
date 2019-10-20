using UnityEngine;
using Cinemachine;

/// <summary>
/// An add-on module for Cinemachine Virtual Camera that locks the camera's Z co-ordinate
/// </summary>
[ExecuteInEditMode]
[SaveDuringPlay]
[AddComponentMenu("")] // Hide in menu
public class CameraXLock : CinemachineExtension
{
    /// <summary>
    /// The initial x position of the camera
    /// </summary>
    public float XPosition;

    /// <summary>
    /// Overrides the PostPipelineStageCallback using the template pattern to remove any x-position effect
    /// </summary>
    protected override void PostPipelineStageCallback(
        CinemachineVirtualCameraBase vcam,
        CinemachineCore.Stage stage, ref CameraState state, float deltaTime)
    {
        if (stage == CinemachineCore.Stage.Body)
        {
            // Resets the new updated x position to XPosition
            var pos = state.RawPosition;
            pos.x = XPosition;
            state.RawPosition = pos;
        }
    }
}