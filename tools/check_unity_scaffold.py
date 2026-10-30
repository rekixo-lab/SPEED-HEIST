#!/usr/bin/env python3
"""Static Unity repository sanity checks; NOT a Unity compile or device test."""
import json
import pathlib
import re

ROOT = pathlib.Path(__file__).resolve().parents[1]
GAME = ROOT / "Game"
ASSETS = GAME / "Assets"

version = (GAME / "ProjectSettings" / "ProjectVersion.txt").read_text()
assert "6000.3.27f1" in version, "Editor version pin missing"

manifest = json.loads((GAME / "Packages" / "manifest.json").read_text())
deps = manifest["dependencies"]
assert deps.get("com.unity.inputsystem") == "1.20.1"
assert deps.get("com.unity.render-pipelines.universal") == "17.3.0"

expected = [
    "Editor/SpeedHeistSetup.cs",
    "Editor/SpeedHeistAndroidBuild.cs",
    "_SpeedHeist/Scripts/RunnerMotor.cs",
    "_SpeedHeist/Scripts/RunnerInput.cs",
    "_SpeedHeist/Scripts/RunnerCamera.cs",
    "_SpeedHeist/Scripts/PrototypeHud.cs",
    "_SpeedHeist/Scripts/RaceFinish.cs",
]
for rel in expected:
    path = ASSETS / rel
    assert path.is_file(), f"Missing {rel}"
    meta = pathlib.Path(str(path) + ".meta")
    assert meta.is_file(), f"Missing {meta}"

seen = set()
meta_files = list(ASSETS.rglob("*.meta"))
for meta in meta_files:
    content = meta.read_text()
    match = re.search(r"^guid: ([a-f0-9]{32})$", content, re.M)
    assert match, f"Invalid or missing Unity GUID: {meta}"
    guid = match.group(1)
    assert guid not in seen, f"Duplicated GUID: {guid}"
    seen.add(guid)

setup = (ASSETS / expected[0]).read_text()
build = (ASSETS / expected[1]).read_text()
assert "Generate Starter Scene" in setup
assert "UniversalRenderPipelineAsset.Create" in setup
assert "Build Android Test APK" in build
assert "BuildPipeline.BuildPlayer" in build
for rel in ["_SpeedHeist/Scripts/RunnerCamera.cs", "_SpeedHeist/Scripts/PrototypeHud.cs"]:
    assert "[SerializeField]" in (ASSETS / rel).read_text(), f"Scene reference serialization missing: {rel}"
assert not (GAME / "Library").exists(), "Do not commit local Unity Library"
print(f"PASS: Unity scaffold, 2 package pins, {len(expected)} scripts, {len(meta_files)} unique .meta GUIDs")
print("NOT TESTED: Unity compilation, scene generation, Android SDK, APK or device gameplay")
