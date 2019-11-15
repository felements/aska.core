# Dotnet Git Version

Solves the problem with executing gitversion at modern ubuntu distros - native git2sharp libs cannot start without lubcurl3 installed (that is conflicted with lubcurl4). This docker image wraps the executables with required old curl libs.

### How to use

```
docekr pull rg.nl-ams.scw.cloud/askaone/gitversion:latest

docker run -it -v {path_to_git_repo_root}:/src rg.nl-ams.scw.cloud/askaone/gitversion:latest  {regular gitversion.tool args}
```

