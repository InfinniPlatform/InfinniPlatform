set path=content\InfinniPlatform\metadata

::Upload assemblies and metadata
pushd Assemblies
InfinniPlatform.Utils.exe upload %path% withMetadata
popd