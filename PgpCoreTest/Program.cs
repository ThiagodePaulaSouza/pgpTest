using PgpCore;
using System.Diagnostics;
using System.Text;

namespace PgpCoreTest
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            Stopwatch sw_gen = new();
            sw_gen.Start();
            using (PGP gen = new())
            {
                gen.GenerateKey(
                    publicKeyFileInfo: new FileInfo($"{Path.GetTempPath()}\\public.gpg"),
                    privateKeyFileInfo: new FileInfo($"{Path.GetTempPath()}\\private.gpg"),
                    username: "email@email.com",
                    password: "",
                    armor: true);
            }
            sw_gen.Stop();

            Console.WriteLine(sw_gen.Elapsed.TotalSeconds); // 0.1438085 | 0.1343805 | 0.1825658

            Stopwatch sw_encrypt = new();
            sw_encrypt.Start();
            string publicKeyFile = await File.ReadAllTextAsync($"{Path.GetTempPath()}\\public.gpg");
            EncryptionKeys publicKey = new(publicKeyFile);
            using PGP pgpCrypt = new(publicKey);
            string responseCrypt = await pgpCrypt.EncryptAsync("String to encrypt");
            sw_encrypt.Stop();

            Stopwatch sw_decrypt = new();
            sw_decrypt.Start();
            string privateKeyFile = File.ReadAllText($"{Path.GetTempPath()}\\private.gpg");
            EncryptionKeys privateKey = new(privateKeyFile, "");
            using PGP pgpDecrypt = new(privateKey);
            string response = await pgpDecrypt.DecryptArmoredStringAsync(responseCrypt);
            sw_decrypt.Stop();
            
            Console.WriteLine(sw_encrypt.Elapsed.TotalSeconds); // 0.0377517 | 0.0378626 | 0.0349491
            Console.WriteLine(sw_decrypt.Elapsed.TotalSeconds); // 0.0225971 | 0.0224786 | 0.0255589

            Stopwatch sw_encrypt2 = new();
            sw_encrypt2.Start();
            string publicKeyFile2 = await File.ReadAllTextAsync($"{Path.GetTempPath()}\\public.gpg");
            EncryptionKeys publicKey2 = new(publicKeyFile2);
            using PGP pgpCrypt2 = new(publicKey2);
            string testJson = @"
{
  ""empresa"": {
    ""nome"": ""Tech Solutions"",
    ""endereco"": {
      ""rua"": ""Avenida Paulista"",
      ""numero"": 1234,
      ""cidade"": ""São Paulo"",
      ""estado"": ""SP"",
      ""cep"": ""01310-100""
    },
    ""contato"": {
      ""telefone"": ""+55 11 98765-4321"",
      ""email"": ""contato@techsolutions.com""
    },
    ""departamentos"": [
      {
        ""nome"": ""Desenvolvimento"",
        ""funcionarios"": [
          {
            ""id"": 1,
            ""nome"": ""Carlos Silva"",
            ""cargo"": ""Engenheiro de Software"",
            ""salario"": 12000,
            ""skills"": [""Java"", ""C#"", ""SQL""],
            ""projetos"": [
              {
                ""id"": 101,
                ""nome"": ""Sistema de Gestão"",
                ""tecnologias"": [""React"", ""Node.js"", ""PostgreSQL""]
              },
              {
                ""id"": 102,
                ""nome"": ""Aplicativo Mobile"",
                ""tecnologias"": [""Flutter"", ""Firebase""]
              }
            ]
          },
          {
            ""id"": 2,
            ""nome"": ""Mariana Souza"",
            ""cargo"": ""Desenvolvedora Frontend"",
            ""salario"": 9500,
            ""skills"": [""React"", ""CSS"", ""TypeScript""],
            ""projetos"": [
              {
                ""id"": 103,
                ""nome"": ""E-commerce"",
                ""tecnologias"": [""Next.js"", ""Redux""]
              }
            ]
          }
        ]
      },
      {
        ""nome"": ""RH"",
        ""funcionarios"": [
          {
            ""id"": 3,
            ""nome"": ""Fernanda Lima"",
            ""cargo"": ""Analista de RH"",
            ""salario"": 8000,
            ""responsabilidades"": [""Recrutamento"", ""Treinamento"", ""Benefícios""]
          }
        ]
      }
    ]
  },
  ""produtos"": [
    {
      ""id"": 201,
      ""nome"": ""Software ERP"",
      ""categoria"": ""Sistemas Empresariais"",
      ""preco"": 50000,
      ""disponibilidade"": true
    },
    {
      ""id"": 202,
      ""nome"": ""Aplicativo de Delivery"",
      ""categoria"": ""Mobile"",
      ""preco"": 15000,
      ""disponibilidade"": false
    }
  ],
  ""clientes"": [
    {
      ""id"": 301,
      ""nome"": ""Loja ABC"",
      ""contato"": {
        ""telefone"": ""+55 11 99876-5432"",
        ""email"": ""contato@lojaabc.com""
      },
      ""compras"": [
        {
          ""produto_id"": 201,
          ""data"": ""2024-01-15"",
          ""valor"": 50000
        }
      ]
    },
    {
      ""id"": 302,
      ""nome"": ""Supermercado XYZ"",
      ""contato"": {
        ""telefone"": ""+55 21 98877-6655"",
        ""email"": ""contato@supermercadoxyz.com""
      },
      ""compras"": [
        {
          ""produto_id"": 202,
          ""data"": ""2024-02-20"",
          ""valor"": 15000
        }
      ]
    }
  ]
}";
            string responseCrypt2 = await pgpCrypt2.EncryptAsync(testJson);
            sw_encrypt2.Stop();

            Stopwatch sw_decrypt2 = new();
            sw_decrypt2.Start();
            string privateKeyFile2 = File.ReadAllText($"{Path.GetTempPath()}\\private.gpg");
            EncryptionKeys privateKey2 = new(privateKeyFile2, "");
            using PGP pgpDecrypt2 = new(privateKey2);
            string response2 = await pgpDecrypt2.DecryptArmoredStringAsync(responseCrypt2);
            sw_decrypt2.Stop();

            Console.WriteLine(sw_encrypt2.Elapsed.TotalSeconds); // 0.0197343 | 0.0294875 | 0.0054435
            Console.WriteLine(sw_decrypt2.Elapsed.TotalSeconds); // 0.0105405 | 0.009269 | 0.0069361
        }
    }
}

