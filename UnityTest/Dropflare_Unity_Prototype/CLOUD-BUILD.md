# Teste Android na nuvem

Projeto: `UnityTest/Dropflare_Unity_Prototype`
Unity: `6000.0.60f1`
Cena: `Assets/Scenes/Main.unity` (já habilitada em EditorBuildSettings)
Android: APK, ARM64/IL2CPP, paisagem, API mínima 26.
Aplicativo separado: `com.bravikstudios.dropflare.unitytest`.

## Unity Build Automation
1. Entrar em https://cloud.unity.com/ com o Unity ID do proprietário.
2. Criar/selecionar um projeto Unity Cloud e abrir DevOps > Build Automation.
3. Usar somente o plano gratuito. Não cadastrar cobrança nem habilitar excedentes pagos.
4. Usar Git manual com URL HTTPS pública; não precisa fornecer token nem chave SSH.
5. Selecionar a branch `main` e o subdiretório acima.
6. Criar configuração Android, Unity 6000.0.60f1, APK, uma compilação manual. Desativar builds automáticos por push para preservar a franquia.
7. O pré-processador `PrototypeBuild` configura identificador, orientação e arquitetura durante o build.
8. Conferir a franquia disponível antes de iniciar e interromper se houver solicitação de pagamento. Baixar o APK somente após build bem-sucedido.

## Alternativa para ambiente Unity licenciado
`Unity -batchmode -nographics -quit -projectPath CAMINHO -buildTarget Android -executeMethod PrototypeBuild.BuildAndroid -logFile build.log`

## Estado real
Os fontes foram preparados sem executar o Editor Unity. Ainda não houve importação, compilação C#, execução ou APK verificado. Acesso à conta Unity confirmado; conexão pública e primeiro build ainda precisam ser verificados. GitHub Actions não foi habilitado: a licença Personal em CI exige ativação que não pode ser presumida disponível sem Unity Hub. Nenhum serviço pago foi contratado.
