{
  inputs = {
    nixpkgs.url = "github:nixos/nixpkgs?ref=nixpkgs-unstable";
    flake-utils.url = "github:numtide/flake-utils";
    rosetta.url = "github:lucaaaaum/rosetta";
  };

  outputs =
    {
      self,
      nixpkgs,
      flake-utils,
      rosetta,
    }:
    flake-utils.lib.eachDefaultSystem (
      system:
      let
        pkgs = import nixpkgs {
          inherit system;
        };
      in
      with pkgs;
      {
        devShells = {
          default = mkShell {
            buildInputs = with pkgs; [
              dotnet-sdk_10
              rosetta.packages.${system}.default
            ];
          };
        };

        packages.default = pkgs.buildDotnetModule {
          pname = name;
          version = version;
          src = ./.;
          projectFile = projectPathString;
          nugetDeps = ./deps.nix;
        };
      }
    );
}
