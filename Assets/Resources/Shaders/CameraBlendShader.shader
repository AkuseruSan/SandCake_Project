﻿Shader "CameraBlend/Mask" {

	SubShader{
		// Render the mask after regular geometry, but before masked geometry and
		// transparent things.

		Tags{ "Queue" = "Geometry+100" }

		// Don't draw in the RGBA channels; just the depth buffer

		ColorMask 0
		//ZTest Less
		ZWrite On

		// Do nothing specific in the pass:

		Pass{}
	}
}