#!/bin/bash

# Script para configurar certificado HTTPS de desenvolvimento para Docker

echo "Configurando certificado HTTPS de desenvolvimento..."

# Criar diretório se não existir
mkdir -p ~/.aspnet/https

# Gerar certificado de desenvolvimento
dotnet dev-certs https --export-path ~/.aspnet/https/aspnetapp.pfx --password ""

echo "Certificado gerado em ~/.aspnet/https/aspnetapp.pfx"
echo ""
echo "Para confiar no certificado (opcional):"
echo "  dotnet dev-certs https --trust"
echo ""
echo "Agora você pode executar: docker-compose up"
