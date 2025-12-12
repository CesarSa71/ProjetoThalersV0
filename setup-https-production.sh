#!/bin/bash

# Script para configurar certificado HTTPS para PRODUÇÃO
# IMPORTANTE: Este script é apenas um exemplo. Em produção real, você deve usar
# certificados válidos de uma Autoridade Certificadora (CA) confiável.

echo "=========================================="
echo "Configuração HTTPS para PRODUÇÃO"
echo "=========================================="
echo ""
echo "⚠️  ATENÇÃO: Para produção, você DEVE usar certificados válidos!"
echo ""
echo "Opções recomendadas:"
echo ""
echo "1. Let's Encrypt (gratuito e confiável):"
echo "   - Use certbot para gerar certificados"
echo "   - Ou use um reverse proxy (nginx/traefik) com Let's Encrypt"
echo ""
echo "2. Certificado fornecido pelo seu provedor de hospedagem"
echo ""
echo "3. Certificado comercial de uma CA confiável"
echo ""
echo "=========================================="
echo ""
read -p "Deseja continuar com a configuração básica? (s/N): " resposta

if [[ ! $resposta =~ ^[Ss]$ ]]; then
    echo "Operação cancelada."
    exit 0
fi

# Criar diretório para certificados
mkdir -p ./certs

echo ""
echo "Para usar um certificado existente:"
echo "1. Coloque seu arquivo .pfx em: ./certs/aspnetapp.pfx"
echo "2. Configure a variável CERTIFICATE_PASSWORD no seu ambiente"
echo ""
echo "Exemplo de uso com docker-compose:"
echo "  CERTIFICATE_PASSWORD='sua_senha' docker-compose -f docker-compose.production.yml up"
echo ""
echo "=========================================="
echo "Opção: Gerar certificado auto-assinado (APENAS PARA TESTES)"
echo "=========================================="
read -p "Gerar certificado auto-assinado para testes? (s/N): " gerar_cert

if [[ $gerar_cert =~ ^[Ss]$ ]]; then
    echo ""
    echo "Gerando certificado auto-assinado..."
    read -sp "Digite uma senha para o certificado (ou Enter para sem senha): " senha
    echo ""
    
    if [ -z "$senha" ]; then
        dotnet dev-certs https --export-path ./certs/aspnetapp.pfx --password ""
        echo "Certificado gerado sem senha em ./certs/aspnetapp.pfx"
    else
        dotnet dev-certs https --export-path ./certs/aspnetapp.pfx --password "$senha"
        echo "Certificado gerado com senha em ./certs/aspnetapp.pfx"
        echo "⚠️  Configure CERTIFICATE_PASSWORD='$senha' ao executar docker-compose"
    fi
    
    echo ""
    echo "⚠️  LEMBRE-SE: Certificados auto-assinados NÃO são adequados para produção!"
    echo "   Os navegadores mostrarão avisos de segurança."
fi

echo ""
echo "Configuração concluída!"
echo ""
echo "Próximos passos:"
echo "1. Obtenha um certificado válido de uma CA confiável"
echo "2. Coloque o arquivo .pfx em ./certs/aspnetapp.pfx"
echo "3. Configure CERTIFICATE_PASSWORD se necessário"
echo "4. Execute: docker-compose -f docker-compose.production.yml up"
