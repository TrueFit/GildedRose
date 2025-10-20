#!/bin/bash

echo "Cleaning up Gilded Rose Monorepo..."

rm -rf node_modules && rm -rf apps/backend/node_modules \
    && rm -rf apps/frontend/node_modules && rm -rf packages/shared-types/node_modules