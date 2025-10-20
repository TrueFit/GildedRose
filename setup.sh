#!/bin/bash

echo "Setting up Gilded Rose Monorepo..."

# Check if Node.js is installed
if ! command -v node &> /dev/null; then
    echo "❌ Node.js is not installed. Please install Node.js >= 18.0.0"
    exit 1
fi

# Check Node version
NODE_VERSION=$(node -v | cut -d'v' -f2)
REQUIRED_VERSION="18.0.0"

if ! printf '%s\n%s\n' "$REQUIRED_VERSION" "$NODE_VERSION" | sort -V -C; then
    echo "❌ Node.js version $NODE_VERSION is too old. Please install >= $REQUIRED_VERSION"
    exit 1
fi

echo "✅ Node.js version $NODE_VERSION detected"

# Install root dependencies
echo "Installing root dependencies..."
npm install

# Install shared types dependencies first
echo "Installing shared-types dependencies..."
cd packages/shared-types
npm install
echo "🔧 Building shared types..."
npm run build
cd ../..

# Install backend dependencies
echo "Installing backend dependencies..."
cd apps/backend
npm install
cd ../..

# Install frontend dependencies  
echo "Installing frontend dependencies..."
cd apps/frontend
npm install
cd ../..

echo ""
echo "Setup complete!"
echo ""
echo "To start development servers:"
echo "  npm run dev           # Start both frontend and backend"
echo "  npm run dev:backend   # Start only backend (port 3001)"
echo "  npm run dev:frontend  # Start only frontend (port 3002)"
echo ""
echo "Access points:"
echo "  Frontend:        http://localhost:3002"
echo "  Backend API:     http://localhost:3001"
echo "  API Docs:        http://localhost:3001/api/docs"
echo ""