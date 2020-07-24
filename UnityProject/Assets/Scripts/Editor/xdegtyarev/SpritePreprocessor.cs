using UnityEngine;
using UnityEditor;
public class SpritePreprocessor : AssetPostprocessor{
	void OnPreprocessTexture(){
		if (assetPath.Contains ("Textures")) {
			TextureImporter importer = assetImporter as TextureImporter;
			importer.textureType = TextureImporterType.Advanced;
			if(importer.DoesSourceTextureHaveAlpha()){
				//importer.alphaIsTransparency = true;
				//importer.textureFormat = TextureImporterFormat.ARGB32;
			}else{
				//importer.alphaIsTransparency = false;
				//importer.textureFormat = TextureImporterFormat.RGB24;
			}
			importer.npotScale = TextureImporterNPOTScale.None;
			importer.mipmapEnabled = false;
			importer.generateMipsInLinearSpace = false;
			//importer.filterMode = FilterMode.Point;
//			importer.spriteImportMode = SpriteImportMode.Single;
		}
	}
}
